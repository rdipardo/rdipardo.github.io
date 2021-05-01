#r "../_lib/Fornax.Core.dll"
#r "../_lib/Fornax.Seo.dll"

open Html
open Fornax.Seo

type Attribution = { notice: string }

type SiteInfo =
    { title: string
      canonical: string
      headline: string
      description: string
      author: ContentCreator
      credits: Attribution
      postPageSize: int }

let loader (projectRoot: string) (siteContent: SiteContents) =
    let onTheWeb =
        [ "linkedin.com/in/rdipardo"
          "bitbucket.org/rdipardo"
          "dev.to/rdipardo" ]

    let siteInfo =
        { title = "Heredocs"
          canonical = "https://heredocs.io"
          headline = ""
          description = "Reflections, tips, and works in progress"
          author =
              { Name = "Robert Di Pardo"
                Email = "dipardo.r@gmail.com"
                SocialMedia = onTheWeb }
          credits =
              { notice = """
            <div>
                <a rel="license" href="https://creativecommons.org/publicdomain/zero/1.0/">
                    <img style="border-style:none;display:block;margin:0 auto;" src="https://licensebuttons.net/p/zero/1.0/88x31.png" alt="CC0" /></a>
                <div class="license">
                    To the extent possible under law,
                    <a rel="dct:publisher" href="https://www.pexels.com/@startup-stock-photos">
                        <span property="dct:title">Startup Stock Photos</span></a>
                        has waived all copyright and related or neighboring rights to
                        <a href="https://www.pexels.com/photo/person-holding-black-pen-while-sitting-7095"><span property="dct:title">Person Holding Black Pen While Sitting</span></a>.
                </div>
            </div>
        """   }
          postPageSize = 5 }

    siteContent.Add(siteInfo)
    siteContent
