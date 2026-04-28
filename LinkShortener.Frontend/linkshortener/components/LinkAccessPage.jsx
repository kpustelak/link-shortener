import { useCallback, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { apiFetch, normalizeDestinationUrl } from "../src/runtimeConfig.js";

export default function LinkAccessPage() {
  const { shortenedUrl } = useParams();
  const navigate = useNavigate();

  const [isLoading, setIsLoading] = useState(true);
  const [requiresPassword, setRequiresPassword] = useState(false);
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const resolveLink = useCallback(async (linkPassword = "") => {
    setIsLoading(true);
    setErrorMessage("");

    try {
      const response = await apiFetch(`/link/${shortenedUrl}/get`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json; charset=utf-8"
        },
        body: JSON.stringify({
          password: linkPassword
        })
      });

      const body = await response.json();

      if (!response.ok || !body?.status || !body?.data) {
        setErrorMessage("Wrong password or link does not exist.");
        return;
      }

      window.location.href = normalizeDestinationUrl(body.data);
    } catch {
      setErrorMessage("Cannot open link right now.");
    } finally {
      setIsLoading(false);
    }
  }, [shortenedUrl]);

  useEffect(() => {
    if (!shortenedUrl) {
      setErrorMessage("Link is invalid.");
      setIsLoading(false);
      return;
    }

    const validateLink = async () => {
      try {
        const response = await apiFetch(`/link/${shortenedUrl}/validation`);
        const body = await response.json();

        if (!response.ok || !body?.status || !body?.data?.isLinkExisting) {
          setErrorMessage("Link does not exist.");
          return;
        }

        if (body.data.isPasswordRequired) {
          setRequiresPassword(true);
          return;
        }

        await resolveLink();
      } catch {
        setErrorMessage("Cannot validate link right now.");
      } finally {
        setIsLoading(false);
      }
    };

    validateLink();
  }, [resolveLink, shortenedUrl]);

  const handleSubmit = async (event) => {
    event.preventDefault();
    await resolveLink(password);
  };

  return (
    <div className="min-h-screen flex items-center justify-center px-6">
      <div className="w-full max-w-md border border-black/10 rounded-2xl p-8">
        <p className="text-3xl font-medium mb-4 text-center">Open short link</p>
        <p className="text-sm text-gray-500 mb-6 text-center">/{shortenedUrl}</p>

        {isLoading && <p className="text-center">Loading...</p>}

        {!isLoading && errorMessage && (
          <p className="text-red-600 text-center mb-4">{errorMessage}</p>
        )}

        {!isLoading && requiresPassword && (
          <form onSubmit={handleSubmit} className="flex flex-col gap-4">
            <input
              type="password"
              placeholder="Enter link password"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              className="border-b-1 text-black text-xl outline-none"
            />
            <button
              type="submit"
              className="bg-black text-white font-bold py-2 rounded-xl transition-colors duration-700 border-2 border-black hover:bg-white hover:text-black"
            >
              OPEN LINK
            </button>
          </form>
        )}

        {!isLoading && (
          <button
            type="button"
            className="mt-6 underline w-full"
            onClick={() => navigate("/")}
          >
            Back to creator
          </button>
        )}
      </div>
    </div>
  );
}
