using BusinessTier.DTO;
using DataTier.Models;
using DataTier.UOW;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BusinessTier.Utilities
{
    public class GoogleSheetApiUtils
    {
        static string[] _scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "FIEP Data";
        static readonly string eventSheet = "Event Data";
        static readonly string groupSheet = "Group Data";
        static readonly int startRow = 2;
        private SheetsService service;

        const String spreadsheetId = "1P5ozlmnkCQhenLCuKEWfTnzv3ijp8S6WnUdA-OqVnnM";
        private readonly IUnitOfWork _unitOfWork;
        public  GoogleSheetApiUtils(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            GoogleCredential credential;
            String path = Path.Combine("keys","google-sheet-credential.json");
            using (var stream =
                new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(_scopes);

                /* credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                     GoogleClientSecrets.Load(stream).Secrets,
                     _scopes,
                     "user",
                     CancellationToken.None,
                     new FileDataStore(credPath, true)).Result;
                 Console.WriteLine("Credential file saved to: " + credPath);*/
            }

            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
                
            });
        }
        public async void UpdateDataToSheet()
        {
            ClearValuesRequest clearValuesRequest = new ClearValuesRequest() ;
            SpreadsheetsResource.ValuesResource.ClearRequest clearEventsRequest 
                    = new SpreadsheetsResource.ValuesResource.ClearRequest(service, clearValuesRequest, spreadsheetId,$"{eventSheet}");
            clearEventsRequest.Execute();

            SpreadsheetsResource.ValuesResource.ClearRequest clearGroupRequest
                    = new SpreadsheetsResource.ValuesResource.ClearRequest(service, clearValuesRequest, spreadsheetId, $"{groupSheet}");
            clearGroupRequest.Execute();


            // The new values to apply to the spreadsheet.
            List<ValueRange> listValueRanges = this.GetListValueRanges();

            // TODO: Assign values to desired properties of `requestBody`:
            BatchUpdateValuesRequest requestBody = new BatchUpdateValuesRequest() {
                ValueInputOption = "USER_ENTERED",
                Data = listValueRanges,
            };

            SpreadsheetsResource.ValuesResource.BatchUpdateRequest request = service.Spreadsheets.Values.BatchUpdate(requestBody, spreadsheetId);

            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            //BatchUpdateValuesResponse response = request.Execute();
            request.ExecuteAsync();
        }
        private List<ValueRange> EventsToValueRanges(List<EventStatisticDTO> list)
        {
            List<ValueRange> result = new List<ValueRange>();

            List<object> eventHeaders = this.getHeaders(EventStatisticDTO.GetAllProperties());
            result.Add(new ValueRange()
            {
                Range = $"{eventSheet}!A1:{(char)(64 + eventHeaders.Count())}",
                Values = new List<IList<object>>()
                    {
                        eventHeaders
                    }
            }); ;

            int row = startRow - 1;
            foreach (var dto in list)
            {
                row++;
                String range = $"{eventSheet}!A{row}:{(char)(64+eventHeaders.Count())}";

                List<object> values = new List<object>();
                foreach (var header in eventHeaders)
                {
                    values.Add(EventStatisticDTO.GetPropValue(dto,(string) header));
                }

                result.Add(new ValueRange()
                {
                    Range = range,
                    Values = new List<IList<object>>()
                    {
                        values
                    }
                });
            }

            return result;
        }

        private List<ValueRange> GroupsToValueRanges(List<GroupStatisticDTO> list)
        {
            List<ValueRange> result = new List<ValueRange>();
            List<object> groupHeaders = this.getHeaders(GroupStatisticDTO.GetAllProperties());

            result.Add(new ValueRange()
            {
                Range = $"{groupSheet}!A1:{(char)(64 + groupHeaders.Count())}",
                Values = new List<IList<object>>()
                    {
                        groupHeaders
                    }
            });

            int row = startRow - 1;
            foreach (var dto in list)
            {
                row++;
                String range = $"{groupSheet}!A{row}:{(char)(64 + groupHeaders.Count())}";

                List<object> values = new List<object>();
                foreach (var header in groupHeaders)
                {
                    values.Add(GroupStatisticDTO.GetPropValue(dto, (string)header));
                }

                result.Add(new ValueRange()
                {
                    Range = range,
                    Values = new List<IList<object>>()
                    {
                        values
                    }
                });
            }

            return result;
        }
        private List<ValueRange> GetListValueRanges()
        {
            List<GroupStatisticDTO> groupData = this.GetGroupsData();
            List<EventStatisticDTO> eventData = this.GetEventsData();

            List<ValueRange> objs = new List<ValueRange>();

            objs.AddRange(this.EventsToValueRanges(eventData));
            objs.AddRange(this.GroupsToValueRanges(groupData));

            return objs;
        }

        private List<object> getHeaders(PropertyInfo[] properties)
        {
            List<object> headers = new List<object>();
            foreach (PropertyInfo pi in properties)
            {
                headers.Add(pi.Name);
            }
            return headers;
        }

        private List<EventStatisticDTO> GetEventsData()
        {
            List<EventStatisticDTO> result = new List<EventStatisticDTO>();



            var listActiveEvents = _unitOfWork.Repository<Event>()
                        .FindAllByProperty(x => x.IsDeleted == false && x.IsExpired==false);
            if (listActiveEvents == null)
            {
                return result;
            }
            foreach (var activeEvent in listActiveEvents)
            {
                int followers = _unitOfWork.Repository<EventSubscription>().FindAllByProperty(x => x.EventId == activeEvent.EventId).Count();
                int postCount = _unitOfWork.Repository<Post>().FindAllByProperty(x => x.EventId == activeEvent.EventId).Count();
                result.Add(new EventStatisticDTO()
                {
                    EventID = activeEvent.EventId,
                    EventName = activeEvent.EventName,
                    Followers=followers,
                    PostCount=postCount,
                }) ;

            }
            return result;
        }

        private List<GroupStatisticDTO> GetGroupsData()
        {
            List<GroupStatisticDTO> result = new List<GroupStatisticDTO>();

            var listActiveGroup = _unitOfWork.Repository<GroupInformation>()
                        .FindAllByProperty(x =>x.IsDeleted==false);

            if (listActiveGroup == null)
            {
                return result;
            }

            foreach (var group in listActiveGroup)
            {
                int followers = _unitOfWork.Repository<GroupSubscription>().FindAllByProperty(x => x.GroupId == group.GroupId).Count();

                var eventRepo = _unitOfWork.Repository<Event>();
                int activeEventsCount = eventRepo.FindAllByProperty(x => x.GroupId == group.GroupId && x.IsExpired==false).Count();
                int eventsCount = eventRepo.FindAllByProperty(x => x.GroupId == group.GroupId).Count();

                result.Add(new GroupStatisticDTO()
                {
                    GroupID = group.GroupId,
                    GroupName = group.GroupName,
                    Followers = followers,
                    EventsCount = eventsCount,
                    ActiveEventsCount= activeEventsCount,
                    
                });

            }
            return result;
        }
    }
}
