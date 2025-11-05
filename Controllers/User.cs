using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using dirtbike.api.Models;
using dirtbike.api.Data;
using Enterpriseservices;
using Microsoft.Extensions.WebEncoders.Testing;
namespace Enterprise.Controllers;


public static class UserEndpoints
{
    
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/User").WithTags(nameof(User));
        Enterpriseservices.Globals.ControllerAPIName = "UserAPI";
        Enterpriseservices.Globals.ControllerAPINumber = "001";
        
        //[HttpGet]
        group.MapGet("/", () =>
        {
           

            using (var context = new DirtbikeContext())
            {
                Enterpriseservices.ApiLogger.logapi(Enterpriseservices.Globals.ControllerAPIName, Enterpriseservices.Globals.ControllerAPINumber, "GET", 1, "Test", "Test");
                return context.Users.ToList();
            }
            
        })
        .WithName("GetAllUsers")
        .WithOpenApi();

        //[HttpGet]
        group.MapGet("/{id}", (int id) =>
        {
            using (var context = new DirtbikeContext())
            {
                Enterpriseservices.ApiLogger.logapi(Enterpriseservices.Globals.ControllerAPIName, Enterpriseservices.Globals.ControllerAPINumber, "GETWITHID", 1, "Test", "Test"); 
                return context.Users.Where(m => m.Id == id).ToList();
            }
        })
        .WithName("GetUserById")
        .WithOpenApi();

        group.MapGet("/userid/{Uidstring}", (string userid) =>
        {
            using (var context = new DirtbikeContext())
            {
                Enterpriseservices.ApiLogger.logapi(Enterpriseservices.Globals.ControllerAPIName, Enterpriseservices.Globals.ControllerAPINumber, "GETWITHID", 1, "Test", "Test"); 
                return context.Users.Where(m => m.Uidstring == userid).ToList();
            }
        })
        .WithName("GetUserByUserId")
        .WithOpenApi();

        //[HttpPut]
        group.MapPut("/password/{userid}", async (int userid, string someplainpassword) =>
        {
            using (var context = new DirtbikeContext())
            {
                User[] someUser = context.Users.Where(m => m.Userid == userid).ToArray();
                context.Users.Attach(someUser[0]);
                someUser[0].Plainpassword = someplainpassword;
                await context.SaveChangesAsync();
                Enterpriseservices.ApiLogger.logapi(Enterpriseservices.Globals.ControllerAPIName, Enterpriseservices.Globals.ControllerAPINumber, "PUTWITHID", 1, "Test", "Test");
                return TypedResults.Accepted("Updated ID:" + userid);
            }
        })
        .WithName("UpdatePassword")
        .WithOpenApi();


        //[HttpPut]
        group.MapPut("/{id}", async (int id, User input) =>
        {
            using (var context = new DirtbikeContext())
            {
                User[] someUser = context.Users.Where(m => m.Userid == id).ToArray();
                context.Users.Attach(someUser[0]);
                if (input.Userid != null) someUser[0].Userid = input.Userid;
                if (input.Fullname != null) someUser[0].Fullname = input.Fullname;
                if (input.Firstname != null) someUser[0].Firstname = input.Firstname;
if (input.Lastname != null) someUser[0].Lastname = input.Lastname;
if (input.Username != null) someUser[0].Username = input.Username;
if (input.Email != null) someUser[0].Email = input.Email;
if (input.Employee != null) someUser[0].Employee = input.Employee;
if (input.Employeeid != null) someUser[0].Employeeid = input.Employeeid;
if (input.Microsoftid != null) someUser[0].Microsoftid = input.Microsoftid;
if (input.Ncrid != null) someUser[0].Ncrid = input.Ncrid;
if (input.Oracleid != null) someUser[0].Oracleid = input.Oracleid;
if (input.Azureid != null) someUser[0].Azureid = input.Azureid;
if (input.Plainpassword != null) someUser[0].Plainpassword = input.Plainpassword;
if (input.Hashedpassword != null) someUser[0].Hashedpassword = input.Hashedpassword;
if (input.Passwordtype != null) someUser[0].Passwordtype = input.Passwordtype;
if (input.Jid != null) someUser[0].Jid = input.Jid;
if (input.Profileurl != null) someUser[0].Profileurl = input.Profileurl;
if (input.Role != null) someUser[0].Role = input.Role;
if (input.Companyid != null) someUser[0].Companyid = input.Companyid;
if (input.Resettoken != null) someUser[0].Resettoken = input.Resettoken;
if (input.Resettokenexpiration != null) someUser[0].Resettokenexpiration = input.Resettokenexpiration;
if (input.Btn != null) someUser[0].Btn = input.Btn;
if (input.Iscertified != null) someUser[0].Iscertified = input.Iscertified;
if (input.Groupid1 != null) someUser[0].Groupid1 = input.Groupid1;
if (input.Groupid2 != null) someUser[0].Groupid2 = input.Groupid2;
if (input.Groupid3 != null) someUser[0].Groupid3 = input.Groupid3;
if (input.Groupid4 != null) someUser[0].Groupid4 = input.Groupid4;
if (input.Groupid5 != null) someUser[0].Groupid5 = input.Groupid5;
if (input.Accountstatus != null) someUser[0].Accountstatus = input.Accountstatus;
if (input.Accountactiondate != null) someUser[0].Accountactiondate = input.Accountactiondate;
if (input.Accountactiondescription != null) someUser[0].Accountactiondescription = input.Accountactiondescription;
if (input.Usertwofactorenabled != null) someUser[0].Usertwofactorenabled = input.Usertwofactorenabled;
if (input.Usertwofactortype != null) someUser[0].Usertwofactortype = input.Usertwofactortype;
if (input.Usertwofactorkeysmsdestination != null) someUser[0].Usertwofactorkeysmsdestination = input.Usertwofactorkeysmsdestination;
if (input.Twofactorkeyemaildestination != null) someUser[0].Twofactorkeyemaildestination = input.Twofactorkeyemaildestination;
if (input.Twofactorprovider != null) someUser[0].Twofactorprovider = input.Twofactorprovider;
if (input.Twofactorprovidertoken != null) someUser[0].Twofactorprovidertoken = input.Twofactorprovidertoken;
if (input.Twofactorproviderauthstring != null) someUser[0].Twofactorproviderauthstring = input.Twofactorproviderauthstring;
if (input.Uidstring != null) someUser[0].Uidstring = input.Uidstring;
if (input.Activeprofileurl != null) someUser[0].Activeprofileurl = input.Activeprofileurl;
if (input.Activepictureurl != null) someUser[0].Activepictureurl = input.Activepictureurl;

                await context.SaveChangesAsync();
                Enterpriseservices.ApiLogger.logapi(Enterpriseservices.Globals.ControllerAPIName, Enterpriseservices.Globals.ControllerAPINumber, "PUTWITHID", 1, "Test", "Test");
                return TypedResults.Accepted("Updated ID:" + input.Userid);
            }


        })
        .WithName("UpdateUser")
        .WithOpenApi();

        group.MapPost("/", async (User input) =>
        {
            using (var context = new DirtbikeContext())
            {
                Random rnd = new Random();
                int dice = rnd.Next(1000, 10000000);
                //input.Id = dice;
                context.Users.Add(input);
                await context.SaveChangesAsync();
                Enterpriseservices.ApiLogger.logapi(Enterpriseservices.Globals.ControllerAPIName, Enterpriseservices.Globals.ControllerAPINumber, "NEWRECORD", 1, "TEST", "TEST");
                return TypedResults.Created("Created ID:" + input.Userid);
            }

        })
        .WithName("CreateUser")
        .WithOpenApi();

        group.MapDelete("/{id}", async (int id) =>
        {
            using (var context = new DirtbikeContext())
            {
                //context.Users.Add(std);
                User[] someUsers = context.Users.Where(m => m.Userid == id).ToArray();
                context.Users.Attach(someUsers[0]);
                context.Users.Remove(someUsers[0]);
                Enterpriseservices.ApiLogger.logapi(Enterpriseservices.Globals.ControllerAPIName, Enterpriseservices.Globals.ControllerAPINumber, "DELETEWITHID",1, "TEST", "TEST");
                await context.SaveChangesAsync();
            }

        })
        .WithName("DeleteUser")
        .WithOpenApi();
    }
}

