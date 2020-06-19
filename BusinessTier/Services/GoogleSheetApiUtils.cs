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

namespace BusinessTier.Services
{
    public class GoogleSheetApiUtils
    {
        static string[] _scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "FIEP Data";
        static readonly string eventSheet = "Event Data";
        static readonly string groupSheet = "Group Data";
        static readonly int startRow = 3;
        private SheetsService service;

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
        public void UpdateDataToSheet()
        {
            String spreadsheetId = "1P5ozlmnkCQhenLCuKEWfTnzv3ijp8S6WnUdA-OqVnnM";

            // The new values to apply to the spreadsheet.
            List<ValueRange> listValueRanges = this.GetListValueRanges();

            // TODO: Assign values to desired properties of `requestBody`:
            BatchUpdateValuesRequest requestBody = new BatchUpdateValuesRequest() {
                ValueInputOption = "USER_ENTERED",
                Data = listValueRanges
            };

            SpreadsheetsResource.ValuesResource.BatchUpdateRequest request = service.Spreadsheets.Values.BatchUpdate(requestBody, spreadsheetId);

            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            BatchUpdateValuesResponse response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync();
        }

        private List<ValueRange> GetListValueRanges()
        {
            List<GroupStatisticDTO> groupData = this.GetGroupsData();
            List<EventStatisticDTO> eventData = this.GetEventsData();

            List<ValueRange> objs = new List<ValueRange>(); 

            int row = startRow;
            foreach (var dto in eventData)
            {
                String range = $"{eventSheet}!A{row}:D";
                objs.Add(new ValueRange()
                {
                    Range = range,
                    Values = new List<IList<object>>()
                    {
                        new List<object>()
                        {
                            dto.EventID,
                            dto.EventName,
                            dto.Followers,
                            dto.postCount
                        }
                    }
                });
                row++;
            }

            row = startRow;
            foreach (var dto in groupData)
            {
                String range = $"{groupSheet}!A{row}:E";
                objs.Add(new ValueRange()
                {
                    Range = range,
                    Values = new List<IList<object>>()
                    {
                        new List<object>()
                        {
                            dto.GroupID,
                            dto.GroupName,
                            dto.Followers,
                            dto.eventsCount,
                            dto.activeEventsCount
                        }
                    }
                });
                row++;
            }
            return objs;
        }

        private List<EventStatisticDTO> GetEventsData()
        {
            List<GroupStatisticDTO> result = new List<GroupStatisticDTO>();

            var listActiveEvents = _unitOfWork.Repository<Event>()
                        .FindAllByProperty(x => (DateTime.Compare((DateTime)x.TimeOccur, DateTime.Now) > 0));
            foreach (var activeEvent in listActiveEvents)
            {
                int followers = _unitOfWork.Repository<EventSubscription>().FindAllByProperty(x => x.EventId == activeEvent.EventId).Count();
                int postCount = _unitOfWork.Repository<Post>().FindAllByProperty(x => x.EventId == activeEvent.EventId).Count();
                result.Add(new GroupStatisticDTO()
                {
                    EventID = activeEvent.EventId,
                    EventName = activeEvent.EventName,
                    Followers=followers,
                    postCount=postCount
                }) ;

            }
            return result;
        }

        private List<GroupStatisticDTO> GetGroupsData()
        {
            List<GroupStatisticDTO> result = new List<GroupStatisticDTO>();

            var listActiveGroup = _unitOfWork.Repository<GroupInformation>()
                        .FindAllByProperty(x =>x.IsDeleted==false);
            foreach (var group in listActiveGroup)
            {
                int followers = _unitOfWork.Repository<GroupSubscription>().FindAllByProperty(x => x.GroupId == group.GroupId).Count();

                var eventRepo = _unitOfWork.Repository<Event>();
                int activeEventsCount = eventRepo.FindAllByProperty(x => x.GroupId == group.GroupId && x.IsExpired==true).Count();
                int eventsCount = eventRepo.FindAllByProperty(x => x.GroupId == group.GroupId).Count();

                result.Add(new GroupStatisticDTO()
                {
                    GroupID = group.GroupId,
                    EventName = group.GroupName,
                    Followers = followers,
                    eventsCount = eventsCount,
                    activeEventsCount= activeEventsCount
                });

            }
            return result;
        }
    }
}
