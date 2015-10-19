module forms

open System.Net.Mail
open Suave.Form

type InterestedEmail = {
  Email : MailAddress
}

let interestedEmail : Form<InterestedEmail> = Form ([],[])

type NewApplication = {
  Address : string;
  Documentation : string;
  Owners : string;
  Developers : string;
  Notes : string;
}

let newApplication : Form<NewApplication> = Form ([],[])
