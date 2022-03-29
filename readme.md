# even horizon example

it can be hard to set up a working event-horizon between two services - use this example to get going.

## description

there are two services:

- producer
- consumer

they each use their own runtime on version 6.1.0 and the dolittle dotnet sdk version 9.1.1

the producer creates two events every time it starts - one internal and one public.

the consumer creates one event every time it starts, and subscribes to the public event from the
producer.

both of the services are single-tenant and get their tenant from a configuration.

the producer's runtime has an `event-horizon-consents.json` file that defines the consumer's right
to access the stream of public events.

the consumer has the subscription to the public event stream defined in `PublicEventSubsciption.cs`

## how to run

### runtimes

```console start the runtimes
$ cd src
$ docker-compose up
```

### consumer

```console start the consumer
$ cd src/consumer
$ dotnet run
```

### producer

```console start the producer
$ cd src/producer
$ dotnet run
```

you will see a line outputting from the consumer every time the producer is started.