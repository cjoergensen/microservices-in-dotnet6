## OpenTelemetry

This is how to run ZipKin it in a docker container

```
docker run --rm -d -p 9411:9411 --name zipkin openzipkin/zipkin
```

ZipKin will now be avaible on http://localhost:9411
