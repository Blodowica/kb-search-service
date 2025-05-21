import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    vus: 10,        // number of virtual users
    duration: '30s' // test duration
};

const BASE_URL = 'http://localhost:5000/api/Movies';

export default function () {
    // Test MovieById endpoint
    let res1 = http.get(`${BASE_URL}/MovieById?movieId=671`);
    check(res1, { 'MovieById status is 200': (r) => r.status === 200 });

    // Test MovieBySearch endpoint
    let res2 = http.get(`${BASE_URL}/MovieBySearch?query=star&page=1`);
    check(res2, { 'MovieBySearch status is 200': (r) => r.status === 200 });

    // Test MovieCredits endpoint
    let res3 = http.get(`${BASE_URL}/MovieCredits?movieId=123`);
    check(res3, { 'MovieCredits status is 200': (r) => r.status === 200 });

    // Add small sleep to simulate user think time
    sleep(1);
}
