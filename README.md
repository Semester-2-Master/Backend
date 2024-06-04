# Pollen Project server
The Pollen Project server receives pictures from the pollen trap device and stores them for further processing. It was designed to run on Microsoft Azure cloud, therefore an Azure account is required.
## Setup instructions for Microsoft Azure
### Storage Account
Follow the official documentation to [create a storage account](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-create) and a [blob container](https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-portal#create-a-container). Go to Access keys at the storage account's settings and copy the Connection string.
### App Service
The Docker image is built by GitHub Actions and is available on GitHub Container Registry. Follow the official documentation to [create a Web App](https://learn.microsoft.com/en-us/azure/app-service/tutorial-custom-container?tabs=azure-portal&pivots=container-linux#v-create-the-web-app) with B1 Basic pricing tier and container registry as a source. Create the following environment variables with correct values from the storage account:
* `AZURESTORAGE_CONNECTION_STRING`
* `AZURESTORAGE_CONTAINER_NAME`
