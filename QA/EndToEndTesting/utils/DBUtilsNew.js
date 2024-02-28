module.exports = function () {
    const sql = require('mssql');

    var config = {
               server: 'SNAVNSQLLVIS012',
               database: 'terminal',
               user: 'AppDev',
               password: 'Terminal@STAGE',
               options: {
                   encrypt: true
               }
           };
this .ConnectDBAsync =  async    function(sqlquery) {
     
    const sql = require('mssql');
    var connectionPool=  new sql.ConnectionPool(config);
    
    await connectionPool.connect();
    
    const result = await connectionPool.request().query(sqlquery);
    
    connectionPool.close();

    return result;

};

}