module.exports = function () {

    var waitActions = require('../commons/waitActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    
    var waitActions = new waitActions();
    var inputBoxActions = new inputBoxActions(); 



    //Test Grid Filters.
    this.filter = function (element, text, result_row) {
        if (typeof element !== 'undefined') {
            inputBoxActions.type(element, text);
            waitActions.waitForElementIsDisplayed(result_row);
            this.validateGirdData(result_row, text);
            return this;
        }
    }

    this.filter = function (element, text) {
        if (typeof element !== 'undefined') {
            inputBoxActions.type(element, text);
            return this;
        }
    }


    this.validateGirdData = function (resultRow, dataValidate) {
        resultRow.getText().then(function _onSuccess(text) {
            console.log(text);
            expect(text).toContain(dataValidate);
        }
        ).catch(function _onFailure(err) {
            console.log(err);
        })
    }
}