import React, { useState, useEffect } from 'react';
import axios from 'axios';

function MyComponent() {
  const [data, setData] = useState(null);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const response = await axios({
        method: 'get',
        url: 'https://api.example.com/data',
        headers: {
          'Content-Type': 'application/json'
        }
      });
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