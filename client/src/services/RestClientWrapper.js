const BASE_URL = 'http://localhost:5094/api/';

export const get = async (url, headers = {}) => {
  const options = {
    method: 'GET',
    headers: headers
  };
  const response = await fetchData(BASE_URL + url, options);
  return response;
};


export const post = async (url, body = {}, headers = {}) => {
  const options = {
    method: 'POST',
    headers: headers,
    body: body,
  };

  const response = await fetchData(BASE_URL + url, options);
  return response;
};


export const fetchData = async (url, options = {}) => {
  const { method = 'GET', body, headers } = options;

  const response = await fetch(url, {
    method,
    headers: {
      'Content-Type': 'application/json',
      ...headers,
    },
    body: body && JSON.stringify(body),
  });

  const data = await response.json();

  return data;
};
