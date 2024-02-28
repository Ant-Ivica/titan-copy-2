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
     this .ConnectDB =      function(sqlquery) {
        return new Promise(function (fulfill, reject) {
            var connection = new sql.ConnectionPool(config);
            connection.connect((err) => {
                if (err) reject(err);
        
                connection.request()
            .query(sqlquery, (err, recordeset) => {
        
                if (err) reject(err);
                else fulfill(recordeset);
            });
        });
        
        })

}

};
