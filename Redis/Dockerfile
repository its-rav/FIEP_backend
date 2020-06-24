FROM redis:alpine

COPY /Redis/conf/ /usr/local/etc/redis/

EXPOSE 6379

CMD [ "redis-server", "/usr/local/etc/redis/redis.conf" ]