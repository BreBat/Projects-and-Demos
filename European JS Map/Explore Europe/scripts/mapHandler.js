//Matrix the same size as the map images that keeps track
//of which country is where, used to optimize country clicking
var hitRegions = [];

//To prevent a bunch of error spam before things are loaded
var mapSetupComplete = false;

//Returns the country code of the country under the given coords
//Returns null if there is no country
function getCountryCodeAtCoord(x, y)
{
    //Error avoidance
    if (mapSetupComplete == false ||
        x < 0 || x >= mapWidth ||
        y < 0 || y >= mapHeight)
        return null;

    var countryClicked = hitRegions[x][y];

    if (countryClicked == "") return null;
    return countryClicked;
}

//Detects if the user clicked on a country in the map
//If not, nothing happens
//If so, send them to the page for that country
function mapClick(event)
{
    //Get mouse coordinates within the map display
    var x = event.offsetX;   var y = event.offsetY;

    //Get the associated country code at that coordinate
    var countryCode = getCountryCodeAtCoord(x, y);

    if (countryCode != null)
    {
        window.location.href = "country-info.php?alpha=" + countryCode;
    }
}

//Displays the name of the country being hovered over
function mapHover(event)
{
    //Get mouse coordinates within the map display
    var x = event.offsetX;   var y = event.offsetY;

    var countryCode = getCountryCodeAtCoord(x, y);

    var hoverText = document.getElementById("hover_indicator");

    if (countryCode != null)
    {
        var canvasID = "layer_" + countryCode;
        var countryName = document.getElementById(canvasID).getAttribute('name');
        hoverText.innerHTML = "Explore: " + countryName;
        document.body.style.cursor = 'pointer';
    }
    else
    {
        hoverText.innerHTML = "Explore: ...";
        document.body.style.cursor = 'default';
    }
}

//Show the hovertext to assist the user when their mouse is on the map...
function mapEnter()
{
    var hoverText = document.getElementById("hover_indicator");
    hoverText.hidden = false;
}

//...and hide the hovertext when the user is off of the map
function mapExit()
{
    var hoverText = document.getElementById("hover_indicator");
    hoverText.hidden = true;
    document.body.style.cursor = 'default';
}

//Gets the country-codes.json info from the server
//this info will be used to interact with the html which is generated using said file
//as well as drawing the map object
function getCountryCodesJSON()
{
    var xhttpr = new XMLHttpRequest();
    var url = "data/country-codes.json";
    xhttpr.open("GET", url, true);
    xhttpr.send();

    xhttpr.onreadystatechange = function() 
    {
        if (this.readyState == 4 && this.status == 200)
        {
            var countryCodesRaw = JSON.parse(this.responseText);
            drawMap(countryCodesRaw);
        }
    };
}

//Called once the XMLHTTPRequest is satisfied, uses the info
//retrieved by the request to draw the map using info about the html file
function drawMap(countryList)
{
    //Prepare a matrix the same size as the map image
    //That will keep track of which country is on which pixel
    for (var i = 0; i < mapWidth; i++)
    {
        hitRegions[i] = [];
        for (var h = 0; h < mapHeight; h++)
        {
            hitRegions[i][h] = "";
        }
    }

    for (i in countryList)
    {
        //Grab the canvas and image objects from the html
        var canvasName = "layer_" + countryList[i].alpha3Code;
        var imageName  = "img_" + countryList[i].alpha3Code;

        var canvas = document.getElementById(canvasName);
        var image = document.getElementById(imageName);

        //Try to draw the image onto the canvas
        if (canvas == null || image == null) continue;
        else
        {
            try
            {
                var context = canvas.getContext("2d");
                context.drawImage(image, 0, 0, mapWidth, mapHeight);

                //Create an internal map of where each country has coordinates in the image
                var layerData = context.getImageData(0,0,mapWidth,mapHeight).data;

                for (var w = 0; w < mapWidth; w++)
                {
                    for (var h = 0; h < mapHeight; h++)
                    {
                        //Each pixel has 4 components, and were interested in the alpha channel
                        var index = (4*mapWidth*h) + (w*4) - 1;
                        if (layerData[index] > 50)
                            hitRegions[w][h] = countryList[i].alpha3Code;
                    }
                }
            }
            catch
            {
                console.log("WARNING: Could not configure map for " + countryList[i].name);
            }
        }
    }
    mapSetupComplete = true;
}

function init()
{
    getCountryCodesJSON();
}
window.addEventListener('load', init);