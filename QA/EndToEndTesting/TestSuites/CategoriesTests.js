describe('Test Execution on TOWER Cateogories Page', function () {

  'use strict';

  var testData = require('../resources/testData.json');
  var categoriesPage = require('../pages/Categories.js');
  var categoriesPage = new categoriesPage();

  beforeEach(function () {
    categoriesPage.openSearchPage(testData.search.homeUrl);
    categoriesPage.isPageLoaded();
  });

  it('Verify filter is working successfully', function () {
    categoriesPage.verifyFilterCategoryNameTextBox();
    categoriesPage.verifyFilterApplicationTextBox();
    categoriesPage.verifyFilterTenantTextBox();
  });

  it('Verify AddNewCateogory', function () {
    categoriesPage.verifyAddNewCategory();
    //categoriesPage.waitforloadingrows();
  });

  it('Verify Add and delete outbounddocument', function () {
    categoriesPage.verifyOutboundDocumentIcon();
  });

  it('Verify Add and delete subscriptions', function () {
    categoriesPage.verifySubscriptionsIcon();
  });

  it('Verify Delete Category', function () {
    categoriesPage.verfiyDeleteCategory();
  });

  it('Verify MessageTypeNameTextBox filter', function () {
    categoriesPage.verifyMessageTypeNameTextBox();
  });

  it('Verify MessageTypeDescriptionTextBox filter', function () {
    categoriesPage.verifyMessageTypeDescriptionTextBox();
  });

  it('Verify TenantTextBox filter', function () {
    categoriesPage.verifyTenantTextBox();
  });

});