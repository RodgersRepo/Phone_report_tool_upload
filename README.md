# Cisco Phone Report Tool Upload Server

A dot net core minimal api application that allows phones or the CUCM "PRT Tool" to upload gun zipped tar files of logs. Ported from the Cisco PHP [script.](https://help.webex.com/en-us/article/ttk1teb/Phone-features-and-setup-on-Unified-CM#concept-template_87e9cc00-7009-464e-a04c-36b128d8316b)

Full instructions on how to set up a "Customer support upload URL" are contained in the link above. Make sure the URL you place in the text field is:
```sh
http://<YOU SERVER URL AND PORT>/upload
```
## Installation
### To VSCODE
For an app this simple might as well just create a new web app project and then paste the contents of **Program.cs** into your project.
```sh
dotnet new create web
```
When you are ready to publish:
```sh
dotnet publish -c Release -o ./publish
```
Then copy the **publish** folder to your server. Please copy the **web.config** attribute **maxAllowedContentLength** to your published **web.config** file. Cisco seem to be recommending file upload sizes of at least 20MB.

## Usage

### Configuring CUCM for this web app

Follow the Cisco instructions to include a customer support upload URL for your phone endpoints registered to CUCM. You can set this at the device level, Common Phone Profile window or Enterprise Phone Configuration window on CUCM.
[Use Problem Report Tool](https://help.webex.com/en-us/article/ttk1teb/Phone-features-and-setup-on-Unified-CM#concept-template_87e9cc00-7009-464e-a04c-36b128d8316b)

## Caveats
This application has only been tested in a lab enviroment with a single CUCM publisher. Maximium file size tested was 6285 KB.

## Credits and references

#### [Use Problem Report Tool](https://help.webex.com/en-us/article/ttk1teb/Phone-features-and-setup-on-Unified-CM#concept-template_87e9cc00-7009-464e-a04c-36b128d8316b)
Guide to configuring a problem report tool URL.

----

