import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { fetchData } from '../services/psAuditing.service';
import { setScope } from './modules/psAuditingActions'; // Assuming action creators are named this way
import ReportingRowDetail from './modules/psReporting/reporting-row-detail';
function MyComponent() {
  const scope = useSelector(state => state.scope);
  const dispatch = useDispatch();
  useEffect(() => {
    const getData = async () => {
      try {
        const response = await fetchData();
        dispatch(setScope(response.data)); // Adjust according to actual data structure
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };
    getData(); // Call getData on component mount
  }, [dispatch]);
  return (
    <div>
      <ReportingRowDetail data={scope} />
      {/* Component UI goes here */}
      {/* Display data or handle it as needed */}
    </div>
  );
}
export default MyComponent;