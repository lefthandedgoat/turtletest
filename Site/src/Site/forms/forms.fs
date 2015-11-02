module forms

open System.Net.Mail
open Suave.Form

type InterestedEmail = {
  Email : MailAddress
}

let interestedEmail : Form<InterestedEmail> = Form ([],[])

type NewUser = {
  Name : string;
  Email : string;
  Password : string;
  RepeatPassword : string;
}

let newUser : Form<NewUser> = Form ([],[])

type NewApplication = {
  Name : string;
  Address : string;
  Documentation : string;
  Owners : string;
  Developers : string;
  Notes : string;
}

let newApplication : Form<NewApplication> = Form ([],[])

type NewSuite = {
  Application : string;
  Name : string;
  Version : string;
  Owners : string;
  Notes : string;
}

let newSuite : Form<NewSuite> = Form ([],[])

type NewTestCase = {
  Application : string;
  Name : string;
  Version : string;
  Owners : string;
  Notes : string;
  Requirements : string;
  Steps : string;
  Expected : string;
  History : string;
  Attachments : string;
}

let newTestCase : Form<NewTestCase> = Form ([],[])
