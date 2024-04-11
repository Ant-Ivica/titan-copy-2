import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { fetchData } from '../actions/psDashboardActions'; // Assuming actions are defined here
import psDashboardService from '../services/psDashboard.service';
import ReportingRowDetail from '../components/ps-reporting/modules/reporting-row-detail';

function MyComponent({ data, fetchData }) {
  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return (
    <div>
      {data ? (
        <div>
          <p>Data loaded successfully!</p>
          <ReportingRowDetail data={data} />
        </div>
      ) : (
        <p>Loading data...</p>
      )}
    </div>
  );
}

const mapStateToProps = (state) => ({
  data: state.psDashboard.data, // Assuming the state structure
});

const mapDispatchToProps = (dispatch) => ({
  fetchData: async () => {
    try {
      const response = await psDashboardService.getData();
      dispatch(fetchData(response.data));
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  },
});

export default connect(mapStateToProps, mapDispatchToProps)(MyComponent);
