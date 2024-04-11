```
import React, { useState, useEffect } from 'react';
import { fetchData } from '../services/psAuditing.service';

function MyComponent() {
  const [scope, setScope] = useState(null);
  const [rootScope, setRootScope] = useState(null);

  useEffect(() => {
    const getData = async () => {
      try {
        const response = await fetchData();
        setScope(response.data); // Adjust according to actual data structure
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    getData(); // Call getData on component mount
  }, []);

  return (
    <div>
      {/* Component UI goes here */}
      {/* Display data or handle it as needed */}
    </div>
  );
}

export default MyComponent;