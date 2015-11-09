module views.partial.sidebar

open Suave.Html
open html_common
open html_bootstrap
open types.read
open types.session

let private side_link link text' count style icon' =
  li [
    aHref link [
      icon icon'
      sidebar_item [text text']
      spanAttr ["class", sprintf "badge badge-%s pull-right" style] [text (string count)]
    ]
  ]

let loginOrLogout session =
  match session with
    | NoSession ->
      aHref "/login" [
        icon "user"
        sidebar_item [text "Login"]
      ]
    | User(user_id) ->
      let user = data.users.getById user_id
      aHref "/logout" [
        icon "user"
        sidebar_item [text (sprintf "Hi %s!" user.Name)]
      ]

let left_sidebar (session : Session) user counts =
  sidebar [
    toggle [ icon "bars" ]
    navblock [
      menu_space [
        content [
          aHref (paths.home_link user) [
            sidebar_logo [
              logo [ emptyText ]
            ]
          ]
          vnavigation [
            side_link (paths.applications_link user) "Applications" counts.Applications "primary" "desktop"
            side_link (paths.suites_link user) "Suites" counts.Suites "success" "sitemap"
            side_link (paths.testcases_link user) "Test Cases" counts.TestCases "prusia" "thumbs-up"
            side_link (paths.executions_link user) "Executions" counts.Executions "danger" "toggle-right"
          ]
        ]
      ]
      menu_space [
        vnavigation [
          li [
            divClass "collapse-button" [
              loginOrLogout session
            ]
          ]
        ]
      ]
    ]
  ]
