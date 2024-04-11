import axios from 'axios';

class PsReportingService {
    private baseURL: string = 'http://example.com/api'; // Set the base URL according to your environment

    // Invalidate Order Data
    invalidateOrderData(orderData: any) {
        return axios.post(`${this.baseURL}/ReportingController/InvalidateOrderData`, orderData, {
            headers: {
                'Content-Type': 'application/json'
            }
        });
    }

    // Get Report Details
    getReportDetails(tenant: string, details: { Fromdate: string, ThroughDate: string }) {
        return axios.post(`${this.baseURL}/ReportingController/GetReportDetails/${tenant}`, details, {
            headers: {
                'Content-Type': 'application/json'
            }
        });
    }

    // Get Report Details Filter
    getReportDetailsFilter(filterSection: string, tenant: string) {
        return axios.get(`${this.baseURL}/ReportingController/GetReportDetailsFilter/${filterSection}/${tenant}`, {
            headers: {
                'Content-Type': 'application/json'
            }
        });
    }

    // Get Report Details by Reference Filter
    getReportDetailsByReferenceFilter(tenant: string, filterDetails: { ReferenceNoType: string, ReferenceNo: string }) {
        return axios.post(`${this.baseURL}/ReportingController/GetReportDetailsbyReferenceFilter/${tenant}`, filterDetails, {
            headers: {
                'Content-Type': 'application/json'
            }
        });
    }

    // Get Tenant
    getTenant() {
        return axios.get(`${this.baseURL}/Security/GetTenant`, {
            headers: {
                'Content-Type': 'application/json'
            }
        });
    }
}

export default new PsReportingService();