 
import React, { useState, useEffect } from 'react';
import * as psSecurityService from '../services/psSecurity.service';

function MyComponent() {
  const [scope, setScope] = useState(null);
  const [rootScope, setRootScope] = useState(null);

  useEffect(() => {
    // Example of replacing direct API call with service call
    psSecurityService.getScopeData()
      .then(data => setScope(data))
      .catch(error => console.error('Failed to fetch scope data', error));

    psSecurityService.getRootScopeData()
      .then(data => setRootScope(data))
      .catch(error => console.error('Failed to fetch root scope data', error));

    // Additional logic to run on component mount can be added here
  }, []);

  return (
    <div>
      {/* Render your component UI here */}
      <p>Scope: {JSON.stringify(scope)}</p>
      <p>Root Scope: {JSON.stringify(rootScope)}</p>
    </div>
  );
}

export default MyComponent;
