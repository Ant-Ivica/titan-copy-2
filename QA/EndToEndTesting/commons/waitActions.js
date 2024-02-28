module.exports = function() {
  //wait till specified time
  this.wait = function(value) {
    browser.sleep(value | 2000);
  };

  //wait for element is displayed
  this.waitForElementIsDisplayed = function(element) {
    if (typeof element !== "undefined") {
      browser.wait(function() {
        return element.isDisplayed();
      }, waitTimeout | 120000)
      .then(function() {}, function() {
        browser.refresh();
    });
    }
  };

  this.waitForElementIsAvailable = function(element) {
    if (typeof element !== "undefined") {
      browser.wait(function() {
        return element.isDisplayed();
      }, waitTimeout | 120000)
      .then(function() {}, function() {
        browser.refresh();
    });
      
    }
  };

  //wait for element is not present
  this.waitForElementIsNotDisplayed = function(element) {
    if (typeof element !== "undefined") {
      browser.wait(function() {
        return !element.isDisplayed();
      }, waitTimeout | 120000);
    }
  };
  //wait for element to be enabled
  this.waitForElementIsEnabled = function(element) {
    if (typeof element !== "undefined") {
      browser.wait(function() {
        return element.isEnabled();
      }, waitTimeout | 120000);
    }
  };
};
