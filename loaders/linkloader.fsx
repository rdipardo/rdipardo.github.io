#r "../_lib/Fornax.Core.dll"

type Link = { site: string; target: string }

let loader (projectRoot: string) (siteContent: SiteContents) =
    siteContent.Add({ site = "Github"; target = "https://github.com/rdipardo" })
    siteContent.Add({ site = "Bitbucket"; target = "https://bitbucket.org/rdipardo" })
    siteContent.Add({ site = "LinkedIn"; target = "https://www.linkedin.com/in/rdipardo" })
    siteContent
