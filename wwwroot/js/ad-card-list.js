import React from "react";
import { createRoot } from "react-dom/client";

const priceFormatter = new Intl.NumberFormat("sv-SE", {
  style: "currency",
  currency: "SEK",
  maximumFractionDigits: 0,
});

const mileageFormatter = new Intl.NumberFormat("sv-SE");

function truncate(text = "", maxLength = 105) {
  return text.length > maxLength
    ? `${text.slice(0, maxLength).trimEnd()}...`
    : text;
}

function AdCard({ ad }) {
  return React.createElement(
    "article",
    { className: "ad-card-react" },
    React.createElement(
      "div",
      { className: "ad-card-react__content" },
      React.createElement("h2", { className: "ad-card-react__title" }, ad.title),
      React.createElement(
        "p",
        { className: "ad-card-react__price" },
        priceFormatter.format(ad.price)
      ),
      React.createElement(
        "p",
        { className: "ad-card-react__description" },
        truncate(ad.shortDescription)
      ),
      React.createElement(
        "div",
        { className: "ad-card-react__meta" },
        React.createElement(
          "span",
          { className: "ad-card-react__meta-item" },
          `${ad.brand} ${ad.model}`
        ),
        React.createElement(
          "span",
          { className: "ad-card-react__meta-item" },
          ad.year
        ),
        React.createElement(
          "span",
          { className: "ad-card-react__meta-item" },
          `${mileageFormatter.format(Number(ad.mileage) || 0)} km`
        )
      )
    ),
    React.createElement(
      "a",
      { className: "ad-card-react__button", href: ad.detailsUrl },
      "View Details"
    )
  );
}

function AdCardList({ ads }) {
  return React.createElement(
    "div",
    { className: "ads-grid" },
    ads.map((ad) => React.createElement(AdCard, { key: ad.id, ad }))
  );
}

const rootElement = document.getElementById("ad-list-react-root");
if (rootElement) {
  const ads = window.__adListData || [];
  createRoot(rootElement).render(React.createElement(AdCardList, { ads }));
}