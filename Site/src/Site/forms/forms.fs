module forms

open System.Net.Mail
open Suave.Form

type InterestedEmail = {
    Email : MailAddress
}

let interestedEmail : Form<InterestedEmail> = Form ([],[])
