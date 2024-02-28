module.exports = function () {

    //select a value from dropdown
    this.select = function (element, value) {
        if (typeof element !== 'undefined') {
            element.isDisplayed().then(function () {
                element.isEnabled().then(function () {
                    if (typeof value !== 'undefined') {
                        element.$('[label="' + value + '"]').click();
                    }
                    return this;
                });
            });
        }
    };

    this.selectDropdownbyNum = function (element, optionNum) {
        if (optionNum) {
            var options = element.all(by.tagName('option'))
                .then(function (options) {
                    options[optionNum].click();
                });
        }
    };


    this.selectDropdownbyValue = function (element, value) {
        if (value) {
            var options = element.all(by.css('option[label="' + value + '"]'))
                .then(function (options) {
                    options[0].click();
                });
        }
    };


};