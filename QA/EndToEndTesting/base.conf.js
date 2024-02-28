var config = require('./G_base.conf.js');
var jasmineReporters = require('jasmine-reporters');
var htmlReporter = require('protractor-html-reporter-2');
var fs = require('fs-extra');
var date = new Date();
var resultsFolderName =  date.getUTCDate() +  "-" + (date.getUTCMonth() + 1) + "-" + date.getUTCFullYear() + " " + date.getUTCHours()+"-"+date.getUTCMinutes()+"-"+date.getUTCSeconds();
var screenShotsFolder = process.cwd() + '/reports/' + resultsFolderName +'/'+ 'chrome'+ '/screenshots/';
var testResultsPath = process.cwd() + '/reports/' + resultsFolderName + '/';
var xmlResultsPath = process.cwd() + '/reports/' + resultsFolderName + '/xml/';
var screenshots = require('protractor-take-screenshots-on-demand');
waitTimeout = 120000;
let {mkdirp} = require('mkdirp');

config.specs = [
	'global.js',
	//'TestSuites/ExampleTestSuite.js',
     //'TestSuites/OrderSummaryTests.js',
	//'TestSuites/ExampleTestSuite.js',
     //'TestSuites/HomeTests.js',
	 'TestSuites/pages_*.js'
];

config.onPrepare = function () {
	screenshots.browserNameJoiner = ' - '; //this is the default
	//folder of screenshots
	screenshots.screenShotDirectory = 'target/screenshots';
	//creates folder of screenshots
	screenshots.createDirectory();

	browser.getCapabilities().then(function (cap) {
		browser.browserName = cap.get('browserName');
		console.log('browserName:', browser.browserName);
	});
	// Default window size
	browser.driver.manage().window().maximize();
	// Default implicit wait
	browser.manage().timeouts().pageLoadTimeout(40000);
    browser.manage().timeouts().implicitlyWait(25000);

	// Angular sync for non angular apps
	browser.ignoreSynchronization = true;
	
	browser.getCapabilities().then(function (cap) {
		fs.emptyDir(screenShotsFolder, function (err) {
			//console.log(err);
		});
	});

	jasmine.getEnv().addReporter(new jasmineReporters.JUnitXmlReporter({
		consolidateAll: true,
		savePath: xmlResultsPath,
		filePrefix: 'xmlresults'
	}));

	jasmine.getEnv().addReporter({
		specDone: function (result) {
			browser.getCapabilities().then(function (caps) {
				var browserName = caps.get('browserName');

				browser.takeScreenshot().then(function (png) {
					if (!fs.existsSync(screenShotsFolder )) {
						mkdirp.sync(screenShotsFolder); // creates multiple folders if they don't exist
					}
					var stream = fs.createWriteStream(screenShotsFolder + browserName + '-' + result.fullName +'.png');
					stream.write(Buffer.from(png, 'base64'));
					stream.end();
				});
			});
		}
	});
};

config.onComplete = function () {
	var browserName, browserVersion;
	var capsPromise = browser.getCapabilities();

	capsPromise.then(function (caps) {
		browserName = caps.get('browserName');
		browserVersion = caps.get('version');
		platform = caps.get('platform');
		testConfig = {
			reportTitle: 'Protractor Test Execution Report',
			outputPath: testResultsPath,
			outputFilename: 'ProtractorTestReport',
			screenshotPath: browserName + '/screenshots',
			testBrowser: browserName,
			browserVersion: browserVersion,
			modifiedSuiteName: false,
			screenshotsOnlyOnFailure: false,
			testPlatform: platform
		};

		new htmlReporter().from(xmlResultsPath+'/xmlresults.xml', testConfig);
	});
};

module.exports = config;