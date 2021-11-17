const fs = require("fs");

const child_process = require("child_process");
const { performance } = require("perf_hooks");




let GeoJsonFile = JSON.parse(fs.readFileSync("data/25.geojson"));

//console.log(GeoJsonFile)

let GeoFeatures = GeoJsonFile.features;

//console.log(GeoFeatures)


let finalFeatures = {
    "type": "FeatureCollection",
    "features": [],
}

let uncheckedFeatures = {
    "type": "FeatureCollection",
    "features": [],
}

let checkedFeatures = {
    "type": "FeatureCollection",
    "features": [],
}

let newFeats = JSON.parse(fs.readFileSync("data/features.geojson"));
console.log(newFeats.features.length);
for (let x = 0; x < newFeats.features.length; x++) {
    const element = newFeats.features[x];
    let dupe = false;
    for (let y = 0; y < checkedFeatures.features.length; y++) {
        const element2 = newFeats.features[y];
        if (x != y) {
            if (element.properties.name === element2.properties.name) {
                dupe = true;
            }
        }
    }
    if (!dupe) {
        checkedFeatures.features.push(newFeats.features[x]);
    }
}
console.log(checkedFeatures.features.length);
fs.writeFileSync("data/checked_features.geojson", JSON.stringify(checkedFeatures));

let features = [];
let first = true;

GeoFeatures.forEach(feature =>
{

    if (first)
    {
        //features.push(`{"_lat":${feature.geometry.coordinates[0][0]},"_long":${feature.geometry.coordinates[0][1]},"_name":${feature.properties.From}}`)
        //features.push(`{"_lat":${feature.geometry.coordinates[1][0]},"_long":${feature.geometry.coordinates[1][1]},"_name":${feature.properties.To}}`)
        first = false;

        features.push({
            "type": "Feature",
            "geometry": {
                "type": "Point",
                "coordinates": [feature.geometry.coordinates[0][0], feature.geometry.coordinates[0][1]]
            },
            "properties": {
                "name": feature.properties.From,
                "location": null,
                "connectsTo": feature.properties.To,
                "connectsFrom": null,
            }
        });

        features.push({
            "type": "Feature",
            "geometry": {
                "type": "Point",
                "coordinates": [feature.geometry.coordinates[feature.geometry.coordinates.length - 1][0], feature.geometry.coordinates[feature.geometry.coordinates.length - 1][1]]
            },
            "properties": {
                "name": feature.properties.To,
                "location": null,
                "connectsTo": null,
                "connectsFrom": feature.properties.From,
            }
        });
    }
    else
    {
        features.push({
            "type": "Feature",
            "geometry": {
                "type": "Point",
                "coordinates": [feature.geometry.coordinates[0][0], feature.geometry.coordinates[0][1]]
            },
            "properties": {
                "name": feature.properties.From,
                "location": null,
                "connectsTo": feature.properties.To,
                "connectsFrom": null,
            }
        });

        features.push({
            "type": "Feature",
            "geometry": {
                "type": "Point",
                "coordinates": [feature.geometry.coordinates[feature.geometry.coordinates.length - 1][0], feature.geometry.coordinates[feature.geometry.coordinates.length - 1][1]]
            },
            "properties": {
                "name": feature.properties.To,
                "location": null,
                "connectsTo": null,
                "connectsFrom": feature.properties.From,
            }
        });
    }
});
//featureString = features.join(',');
//features = JSON.parse('[' + featureString + ']');
//finalFeatures = []
//console.log(features)
let dupes = 1;
let valids = 1;
let cycles = 1;
finalFeatures.features.push(features[0]);



features.forEach(featureChecking =>
{
    uncheckedFeatures.features.push(featureChecking);
    cycles++
    valid = false;
    finalFeatures.features.forEach(element =>
    {
        if (featureChecking.properties.name === element.properties.name && featureChecking.geometry.coordinates[0] === element.geometry.coordinates[0] && featureChecking.geometry.coordinates[1] === element.geometry.coordinates[1])
        {
            valid = false;
            return
        }
        else
        {
            valid = true

        }
    });
    if (valid)
    {
        valids = finalFeatures.features.push(featureChecking)
    }
    else
    {
        dupes++
    }

});
console.log("Final Featurres!")
console.log(finalFeatures)
console.log(`Dupes: ${dupes} - Valids: ${valids} - Cycles: ${cycles}`);


let finalDupe = 0;
finalFeatures.features.forEach(feature =>
{
    let workingNum = 0;
    finalFeatures.features.forEach(innerFeature =>
    {

        if (feature === innerFeature)
        {
            workingNum++;
        }
    });
    if (workingNum > 1)
    {
        finalDupe++
    }
});

let percentCount = 0;
let totalFeats = finalFeatures.features.length;
let lastPerfTime = 0;
let avgTime = 0;
let perfTimes = [0];

let lastPerc = 0;

finalFeatures.features.forEach(feature =>
{
    //console.log(`${percentCount}/${totalFeats}`);
    let perf_bef = performance.now();
    let p = percentCount / totalFeats;
    if (perfTimes.length > 20)
    {
        perfTimes = perfTimes.slice(1);
    }
    var total = 0;
    var count = 0;

    perfTimes.forEach(value =>
    {
        total += value;
        count++;
    });

    avgTime = total / count;
    let percent = (Math.round(100 * (p * 100).toFixed(2)) / 100) + "%";

    let timetoFinish = avgTime * (totalFeats - percentCount);

    if (percentCount % 10 == 0)
    {
        //console.log(percentCount % 10)
        console.log(percent + " - " + msToTime(timetoFinish) + " until finished");
        lastPerc = (Math.round(100 * (p * 100).toFixed(3)) / 100);
        if ((Math.round(100 * (p * 100).toPrecision(2)) / 100) != lastPerc)
        {

        }

    }


    let locationCall = child_process.execSync(`ruby ruby/ocean-name.rb ${feature.geometry.coordinates[1]} ${feature.geometry.coordinates[0]}`);
    let locString = locationCall.toString();
    if (locString.length != 1)
    {
        locString = locString.replace(/=>/g, ":");

        let jsonLoc = JSON.parse(locString);
        //console.log(jsonLoc.name);
        feature.properties.location = jsonLoc.name;
    }
    else
    {
        //console.log("Unknown Sea")
        feature.properties.location = "Unknown Sea";
    }
    percentCount++;
    let perf_aft = performance.now();

    lastPerfTime = perf_aft - perf_bef;
    perfTimes.push(lastPerfTime);
});


console.log(`Final Duplicates: ${finalDupe}`);
fs.writeFileSync("data/features.geojson", JSON.stringify(finalFeatures));
fs.writeFileSync("data/features-inc-dupes.geojson", JSON.stringify(uncheckedFeatures));
fs.writeFileSync("data/ocean-points.json", JSON.stringify(finalFeatures.features));

let nodeVer = child_process.execSync("node -v");

console.log(nodeVer.toString());

let OceanName = child_process.execSync("ruby ruby/ocean-name.rb 0 0");
let oceanNameString = OceanName.toString();

oceanNameString = oceanNameString.replace(/=>/g, ':');

console.log(OceanName.toString());

let jsonBobj = JSON.parse(oceanNameString);

console.log(jsonBobj);

child_process.exec(`ruby ruby/ocean-name.rb 0 0`, function (err, stdout, stderr)
{
    console.log(stdout);
    console.log(stderr);
});
/*
finalFeatures.features.forEach(feature => {
    execSync(`ruby ruby/ocean-name.rb ${feature.geometry.coordinates[0]} ${feature.geometry.coordinates[1]}`, function (err, stdout, stderr) {
        console.log(stdout);
    });
})

//CallRuby();

async function CallRuby(){
    let featCount = 0
    let running = false;
    while(featCount < finalFeatures.features.length){
        await execSync(`ruby ruby/ocean-name.rb ${finalFeatures.features[featCount].geometry.coordinates[0]} ${finalFeatures.features[featCount].geometry.coordinates[1]}`, function (err, stdout, stderr) {
            console.log(stdout);
            featCount++;
        });
        
        
    }
}*/
function msToTime(duration)
{
    var milliseconds = Math.floor((duration % 1000) / 100),
        seconds = Math.floor((duration / 1000) % 60),
        minutes = Math.floor((duration / (1000 * 60)) % 60),
        hours = Math.floor((duration / (1000 * 60 * 60)) % 24);

    hours = (hours < 10) ? "0" + hours : hours;
    minutes = (minutes < 10) ? "0" + minutes : minutes;
    seconds = (seconds < 10) ? "0" + seconds : seconds;

    return hours + " hours, " + minutes + " minutes";
}

