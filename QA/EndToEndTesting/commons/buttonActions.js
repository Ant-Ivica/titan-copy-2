module.exports = function () {

    //click on an element
    this.click = function (element) {
        if (typeof element !== 'undefined') {
            element.isDisplayed().then(function () {
                element.isEnabled().then(function () {
                    element.click();
                    return this;
                });
            });
        }
    };

    this.selectDropdownbyNum = function ( element, optionNum ) {
        if (optionNum){
          var options = element.all(by.tagName('option'))   
            .then(function(options){
              options[optionNum].click();
            });
        }
      };


};
