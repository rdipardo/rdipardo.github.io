const fs = require('fs');
const minify = require('minify');
const minifyOpts = {
    'html': {
        'html5': false,
        'useShortDoctype': false,
        'keepClosingSlash': true,
        'collapseBooleanAttributes': true,
        'removeAttributeQuotes': false,
        'removeComments': false,
        'removeCommentsFromCDATA': false,
        'removeCDATASectionsFromCDATA': false,
        'removeEmptyElements': false,
        'removeOptionalTags': false,
        'removeStyleLinkTypeAttributes': false,
        'removeScriptTypeAttributes': false
    },
    'css': {
        'compatibility': '*'
    },
    'js': {
        'ecma': 5
    }
}

const [, , fileName] = process.argv
let minified = ''

if (fileName) {
    if ((/(?<!(min))\.((js)|(css)|(html))$/iu).test(fileName)) {
        fs.readFileSync(fileName, 'utf8', (err, data) => {
            if (err) throw err
            minified = data
        })

        minify(fileName, minifyOpts)
            .then(min => console.log(min))
            .catch(err => console.error(err.message))
    }
}

console.log(minified)