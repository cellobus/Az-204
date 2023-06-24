/*
##########################################################################################

#values form previous run for Event Hub
$eventhub="";  # <<<<<--- please provide short name of eventhub from previous run
$groupname="EventHubDemo-RG"

# to avoid name collisions generate unique name for your account
$account="eventgri"+(Get-Random) 

#Create a resource group
New-AzResourceGroup -location eastus -name EventGridDemo-RG

#Enable the Event Grid resource provider
Register-AzResourceProvider -ProviderNamespace Microsoft.EventGrid

#Create a resource group to monitor
New-AzResourceGroup -location eastus -name EventGridMonitoring

#Pull azure subscription id
$subid=(az account show --query id -o tsv)

#Сonfigure event subscription endpoint
$endpoint="/subscriptions/$subid/resourceGroups/$groupname/providers/Microsoft.EventHub/namespaces/$eventhub/eventhubs/$eventhub"

#Create a subscription for the events from Resourece group
New-AzEventGridSubscription -EventSubscriptionName "group-monitor-sub"  -EndpointType "eventhub" -Endpoint $endpoint -ResourceGroup "EventGridMonitoring"

# Update tag of monitoring RG. Required about 45 to appear in subscriber console
Set-AzResourceGroup -name EventGridMonitoring -Tag @{Code=(Get-Random)}

# do not delete the provision resources, it will be required for next step
*/
class Program
    {
        static string endpoint = "https://az204eventgridtopic.swedencentral-1.eventgrid.azure.net/api/events";
        static string key = "+KQN4DT9vRfuH7WG+9ldE0k0dmlSbpwy3GpsOo8L640=";

        static void Main(string[] args)
        {
            Send().Wait();

        }

        static async Task Send()
        {
            try {
                EventGridPublisherClient client = new EventGridPublisherClient(
                new Uri(endpoint),
                new AzureKeyCredential(key));
                        // Add EventGridEvents to a list to publish to the topic
                        EventGridEvent egEvent =
                            new EventGridEvent(
                                "ExampleEventSubject",
                                "Example.EventType",
                                "1.0",
                                "This is the event data");

                        // Send the event
                        await client.SendEventAsync(egEvent);
            }
            catch (Exception ex)   {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
