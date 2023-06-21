# massTransit
Repo to get started with Mass Transit

## Purpose
Mass Transit is package used to interface many message queue types behind it like the Azure Service Bus or the open source RabbitMQ. When developing locally it easier to make use of rmq and switch accordingly when on production to the Azure Service Bus.

This repo can be used as a template where `Producer.cs` is loaded as a background service and messages are sent periodically to the `Consumers`. Consumers processes the message in turn.

## Requirements
Setup `RabbitMQ` docker image locally to test it but is not required.

Running `RabbitMQ` container locally with `docker` should be easily achieved by running the below command.

```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management
```

Check `RabbitMQ`'s site for the updated image.

## Running
Say if the application is already running using `dotnet run --no-build Debug` command after `dotnet build`, starting the `RabbitMQ` docker instance should start processing the messages and should be accessible in `localhost:15267/#/queues` or similar.

Check `inmem-queue` branch in case you like to use the in memory queue for hosting the messages.

### Donations
If you like using this repository and like to donate, please visit the below link. This work is made possible with donations like yours. PM for customizations and implementations .

<a href="https://www.buymeacoffee.com/ragavendra"><img src="https://img.buymeacoffee.com/button-api/?text=Buy me a pop&emoji=🥃&slug=ragavendra&button_colour=FFDD00&font_colour=000000&font_family=Cookie&outline_colour=000000&coffee_colour=ffffff" /></a>

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ZKRHDCLG22EJA)