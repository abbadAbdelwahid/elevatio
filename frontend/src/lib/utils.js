
import { clsx } from "clsx";
import { twMerge } from "tailwind-merge"

export function cn(...inputs) {
  return twMerge(clsx(inputs));
}
export function getRoleFromCookie() {
  if (typeof document === "undefined") return null; // SSR check

  const cookie = document.cookie
      .split("; ")
      .find((row) => row.startsWith("role="));

  return cookie?.split("=")[1] || null;
}export function getUserIdFromCookie() {
  if (typeof document === "undefined") return null; // SSR check

  const cookie = document.cookie
      .split("; ")
      .find((row) => row.startsWith("userId="));

  return cookie?.split("=")[1] || null;
}
