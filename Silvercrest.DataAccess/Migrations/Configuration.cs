namespace Silvercrest.DataAccess.Migrations
{
    using Entities;
    using Entities.Enums;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Silvercrest.DataAccess.Model.SLVR_DEVEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        //protected override void Seed(Silvercrest.DataAccess.Model.SLVR_DEVEntities context)
        //{
        //    AddRoles(context);
        //    AddUser(context);
        //    AddContacts(context);
        //    //AddSecretQuestion(context);
        //    AddAccess(context);
        //    AddAccounts(context);
        //    AddFamily(context);
        //    AddReferences(context);
        //}
        //private void AddSecretQuestion(Silvercrest.DataAccess.Model.SLVR_DEVEntities context)
        //{
        //    var questions = context.SecretQuestions.Any();
        //    if (questions)
        //    {
        //        return;
        //    }
        //    List<string> questionsList = new List<string>{
        //        "What was your childhood nickname?",
        //        "In what city did you meet your spouse / significant other?",
        //        "What is the name of your favorite childhood friend?",
        //        "What street did you live on in third grade?",
        //        "What is your oldest sibling’s birthday month and year? (e.g., January 1900)",
        //        "What is the middle name of your oldest child?",
        //        "What is your oldest sibling's middle name?",
        //        "What school did you attend for sixth grade?",
        //        "What was your childhood phone number including area code? (e.g., 000 - 000 - 0000)",
        //        "What is your oldest cousin's first and last name?",
        //        "What was the name of your first stuffed animal?",
        //        "In what city or town did your mother and father meet?",
        //        "Where were you when you had your first kiss?",
        //        "What is the first name of the boy or girl that you first kissed?",
        //        "What was the last name of your third grade teacher?",
        //        "In what city does your nearest sibling live?",
        //        "What is your oldest brother’s birthday month and year? (e.g., January 1900)",
        //        "What is your maternal grandmother's maiden name?",
        //        "In what city or town was your first job?",
        //        "What is the name of the place your wedding reception was held?",
        //        "What is the name of a college you applied to but didn't attend?",
        //        "Where were you when you first heard about 9/11?"};

        //    foreach (var question in questionsList)
        //    {
        //        var secretQuestion = new SecretQuestion();
        //        secretQuestion.Question = question;
        //        context.SecretQuestions.Add(secretQuestion);
        //    }
        //    context.SaveChanges();
        //}

        //private void AddRoles(Silvercrest.DataAccess.Model.SLVR_DEVEntities context)
        //{
        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        //    if (roleManager.Roles.Any())
        //    {
        //        return;
        //    }
        //    roleManager.Create(new IdentityRole { Name = UserRole.Administrator.ToString() });
        //    roleManager.Create(new IdentityRole { Name = UserRole.Client.ToString() });
        //    roleManager.Create(new IdentityRole { Name = UserRole.SuperUser.ToString() });
        //    roleManager.Create(new IdentityRole { Name = UserRole.TeamMember.ToString() });
        //    context.SaveChanges();
        //}

        //private void AddUser(Silvercrest.DataAccess.Model.SLVR_DEVEntities context)
        //{
        //    var userIsAdded = context.Users.Any(x => x.UserName == "user@user.com");
        //    if (userIsAdded)
        //    {
        //        return;
        //    }
        //    var store = new UserStore<ApplicationUser>(context);
        //    var manager = new UserManager<ApplicationUser>(store);
        //    var user = new ApplicationUser { UserName = "user@user.com", Email = "user@user.com", EmailConfirmed = true, IsNeedPasswordGenerate = true };

        //    manager.Create(user, "123456");
        //    manager.AddToRole(user.Id, UserRole.Administrator.ToString());
        //    context.SaveChanges();
        //}
        //private void AddContacts(Silvercrest.DataAccess.Model.SLVR_DEVEntities context)
        //{
        //    var contact = new Contact { SourceIdd = 0, FirmUserGroupId = 0, InsertDate = DateTime.Now, IsActive = false, FirstName = "Richard", MiddleName = "Edgar", LastName = "Gilmore", EmailAddress1 = "mail@mail.com" };
        //    context.Contacts.Add(contact);
        //    contact = new Contact { SourceIdd = 0, FirmUserGroupId = 0, InsertDate = DateTime.Now, IsActive = false, FirstName = "John", MiddleName = "Ronald", LastName = "Brown", EmailAddress1 = "example@mail.com" };
        //    context.Contacts.Add(contact);
        //    contact = new Contact { SourceIdd = 0, FirmUserGroupId = 0, InsertDate = DateTime.Now, IsActive = false, FirstName = "Oleksii", MiddleName = "Sergiy", LastName = "Isak", EmailAddress1 = "aleksey_isak@mail.ru" };
        //    context.Contacts.Add(contact);
        //    context.SaveChanges();
        //}
        //private void AddAccess(Silvercrest.DataAccess.Model.SLVR_DEVEntities context)
        //{
        //    var userIsAdded = context.Acceses.Any();
        //    if (userIsAdded)
        //    {
        //        return;
        //    }
        //    var access = new Access { AccountName = "JOHN SMITH JR. BALANCED ACCOUNT", ShortName = "JSMITHJRBL", Manager = "CATHY JAMESON", IsHaveAccess = false };
        //    context.Acceses.Add(access);
        //    access = new Access { AccountName = "JOHN SMITH JR. BALANCED ACCOUNT", ShortName = "JSMITHJRBL", Manager = "CATHY JAMESON", IsHaveAccess = false };
        //    context.Acceses.Add(access);
        //    access = new Access { AccountName = "JOHN SMITH JR. BALANCED ACCOUNT", ShortName = "JSMITHJRBL", Manager = "CATHY JAMESON", IsHaveAccess = false };
        //    context.Acceses.Add(access);
        //    access = new Access { AccountName = "JOHN SMITH JR. BALANCED ACCOUNT", ShortName = "JSMITHJRBL", Manager = "CATHY JAMESON", IsHaveAccess = true };
        //    context.Acceses.Add(access);
        //    access = new Access { AccountName = "JOHN SMITH JR. BALANCED ACCOUNT", ShortName = "JSMITHJRBL", Manager = "CATHY JAMESON", IsHaveAccess = true };
        //    context.Acceses.Add(access);
        //    access = new Access { AccountName = "JOHN SMITH JR. BALANCED ACCOUNT", ShortName = "JSMITHJRBL", Manager = "CATHY JAMESON", IsHaveAccess = true };
        //    context.Acceses.Add(access);
        //    context.SaveChanges();
        //}
        //private void AddAccounts(Silvercrest.DataAccess.Model.SLVR_DEVEntities context)
        //{
        //    var account = new Account { Name = "First Test Account", MarketValue = 45000, PercentOfTotal = 0.2345 };
        //    context.Accounts.Add(account);
        //    account = new Account { Name = "Second Test Account", MarketValue = 33647, PercentOfTotal = 0.4678 };
        //    context.Accounts.Add(account);
        //    account = new Account { Name = "Third Test Account", MarketValue = 61250, PercentOfTotal = 0.1234 };
        //    context.Accounts.Add(account);
        //    context.SaveChanges();
        //}
        //private void AddFamily(Silvercrest.DataAccess.Model.SLVR_DEVEntities context)
        //{
        //    var family = new Family { Name = "Charity" };
        //    context.Families.Add(family);
        //    family = new Family { Name = "Vatson's" };
        //    context.Families.Add(family);
        //    family = new Family { Name = "Richard" };
        //    context.Families.Add(family);
        //    context.SaveChanges();
        //}
        //private void AddReferences(Silvercrest.DataAccess.Model.SLVR_DEVEntities context)
        //{

        //    for (int i = 0; i < context.Contacts.Count(); i++)
        //    {
        //        context.Contacts.ToList()[i].Accounts = new List<Account>();
        //        context.Contacts.ToList()[i].Family = context.Families.ToList()[i];
        //        context.Contacts.ToList()[i].Accounts.Add(context.Accounts.ToList()[i]);
        //    }
        //}
    }
}
