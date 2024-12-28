/*
Copyright (C) 2024  Rodge Industries 2000
 
     This program is free software: you can redistribute it and/or modify
     it under the terms of the GNU General Public License as published by
     the Free Software Foundation, version 3.
 
     This program is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.
 
     To view the GNU General Public License, see <http://www.gnu.org/licenses/>.
     A copy of the LICENSE can be found in this repository.
    Release Date: 04/12/2024
    Last Updated:      
   
    Change comments:
    Initial realease V1 - RITT   
   
  Author: RodgeIndustries2000 (RITT) with help from Microsoft CoPilot
  ported from the Cisco PHP script to C# .NET 6.0
  https://help.webex.com/en-us/article/ttk1teb/Phone-features-and-setup-on-Unified-CM#concept-template_87e9cc00-7009-464e-a04c-36b128d8316b
    
  Description: This web application is used to upload phone report tool (PRT) files to
  a server from Cisco Callmanager. Cisco recomend adjusting file upload size to 20MB 
  see web.config for this change
*/
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/upload", async (HttpRequest request) =>
{
    // Check that a file was present in the form data
    if (!request.HasFormContentType || !request.Form.Files.Any())
    {
        return Results.Problem("Error: You must select a file to upload.", statusCode: 500);
    }

    // Get rid of quotes around the device name, serial number and username if they exist
    var form = await request.ReadFormAsync();
    var file = form.Files["prt_file"];
  
    if (file == null)
    {
        return Results.Problem("Error: No file found in the request.", statusCode: 500);
    }
    
    // Remove the charage returns from the form data
    var serialno = form["serialno"].ToString().Replace("\r", "").Replace("\n", "");
    var devicename = form["devicename"].ToString().Replace("\r", "").Replace("\n", "");
    Console.WriteLine($"New upload request from {devicename} serial number {serialno}");
    var filename = Path.GetFileName(file.FileName);

    // Create the local directory for the file upload if it dosnt already exsist
    Directory.CreateDirectory(AppContext.BaseDirectory + $"Logs\\{devicename}");
    var fullfilename = Path.Combine(AppContext.BaseDirectory + $"Logs\\{devicename}", filename);
    Console.WriteLine($"Will attemp to save to {fullfilename}");

    try
    {
        using (var stream = new FileStream(fullfilename, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unable to save {ex.ToString()}");
        return Results.Problem("Error: File upload failed.", statusCode: 500);
    }

    return Results.Ok("File uploaded successfully.");
});

app.Run();
