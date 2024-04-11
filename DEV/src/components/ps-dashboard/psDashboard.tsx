```
import React, { useState, useEffect } from 'react';
import psDashboardService from '../services/psDashboard.service';

function MyComponent() {
  const [data, setData] = useState(null);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const response = await psDashboardService.getData();
      setData(response.data);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  return (
    <div>
      {data ? <p>Data loaded successfully!</p> : <p>Loading data...</p>}
    </div>
  );
}

export default MyComponent;