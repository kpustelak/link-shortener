const trimTrailingSlash = (value) => value.replace(/\/+$/, "");
const LOCAL_HTTPS_API = "https://localhost:7162";
const LOCAL_HTTP_API = "http://localhost:5088";

export const getRouterBasePath = () => {
  const envBasePath = import.meta.env.VITE_ROUTER_BASENAME;

  if (envBasePath && envBasePath.trim() !== "") {
    return envBasePath;
  }

  return import.meta.env.BASE_URL || "/";
};

export const getApiBaseUrl = () => {
  const envApiUrl = import.meta.env.VITE_API_URL;

  if (envApiUrl && envApiUrl.trim() !== "") {
    return trimTrailingSlash(envApiUrl);
  }

  return trimTrailingSlash(window.location.origin);
};

export const getApiBaseUrls = () => {
  const values = [];
  const envApiUrl = import.meta.env.VITE_API_URL;

  if (envApiUrl && envApiUrl.trim() !== "") {
    values.push(trimTrailingSlash(envApiUrl));
  }

  values.push(trimTrailingSlash(window.location.origin));
  values.push(LOCAL_HTTPS_API);
  values.push(LOCAL_HTTP_API);

  return [...new Set(values)];
};

export const buildApiUrl = (path) => {
  const normalizedPath = path.startsWith("/") ? path : `/${path}`;
  return `${getApiBaseUrl()}${normalizedPath}`;
};

export const apiFetch = async (path, options = {}) => {
  const normalizedPath = path.startsWith("/") ? path : `/${path}`;
  const baseUrls = getApiBaseUrls();

  let lastError = null;
  let lastApiResponse = null;

  for (const baseUrl of baseUrls) {
    try {
      const response = await fetch(`${baseUrl}${normalizedPath}`, options);
      const contentType = response.headers.get("content-type") ?? "";
      const looksLikeApiResponse = contentType.includes("application/json");

      // Ignore HTML/other non-API responses (e.g. Vite dev server 404 page).
      if (!looksLikeApiResponse) {
        continue;
      }

      lastApiResponse = response;
      return response;
    } catch (error) {
      lastError = error;
    }
  }

  if (lastApiResponse) {
    return lastApiResponse;
  }

  throw lastError ?? new Error("Cannot connect to API.");
};

export const normalizeDestinationUrl = (rawUrl) => {
  const trimmedUrl = rawUrl?.trim() ?? "";

  if (trimmedUrl === "") {
    return "";
  }

  if (/^https?:\/\//i.test(trimmedUrl)) {
    return trimmedUrl;
  }

  return `https://${trimmedUrl}`;
};
