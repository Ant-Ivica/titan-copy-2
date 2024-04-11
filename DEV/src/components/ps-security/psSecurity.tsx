import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import * as psSecurityService from '../services/psSecurity.service';
import { setScope, setRootScope } from './modules/psSecurityStore';
import ReportingRowDetail from '../components/psReporting/ReportingRowDetail';
function MyComponent() {
  const scope = useSelector(state => state.psSecurity.scope);
  const rootScope = useSelector(state => state.psSecurity.rootScope);
  const dispatch = useDispatch();
  useEffect(() => {
    // Example of replacing direct API call with service call
    psSecurityService.getScopeData()
      .then(data => dispatch(setScope(data)))
      .catch(error => console.error('Failed to fetch scope data', error));
    psSecurityService.getRootScopeData()
      .then(data => dispatch(setRootScope(data)))
      .catch(error => console.error('Failed to fetch root scope data', error));
    // Additional logic to run on component mount can be added here
  }, [dispatch]);
  return (
    <div>
      {/* Render your component UI here */}
      <p>Scope: {JSON.stringify(scope)}</p>
      <p>Root Scope: {JSON.stringify(rootScope)}</p>
      <ReportingRowDetail scope={scope} rootScope={rootScope} />
    </div>
  );
}
export default MyComponent;