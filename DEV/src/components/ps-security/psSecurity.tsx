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
    async function fetchData() {
      try {
        const scopeData = await psSecurityService.getScopeData();
        dispatch(setScope(scopeData));
        const rootScopeData = await psSecurityService.getRootScopeData();
        dispatch(setRootScope(rootScopeData));
      } catch (error) {
        console.error('Failed to fetch data', error);
      }
    }
    fetchData();
  }, [dispatch]);
  return (
    <div>
      <p>Scope: {JSON.stringify(scope)}</p>
      <p>Root Scope: {JSON.stringify(rootScope)}</p>
      <ReportingRowDetail scope={scope} rootScope={rootScope} />
    </div>
  );
}
export default MyComponent;