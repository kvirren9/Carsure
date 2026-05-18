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

function normalizeImageUrls(ad) {
  if (Array.isArray(ad.imageUrls) && ad.imageUrls.length > 0) {
    return ad.imageUrls.filter(Boolean).slice(0, 10);
  }

  if (typeof ad.imageUrl === "string" && ad.imageUrl.trim().length > 0) {
    return ad.imageUrl
      .split(/\r|\n|,|;/)
      .map((x) => x.trim())
      .filter(Boolean)
      .slice(0, 10);
  }

  return [];
}

function AdCard({ ad }) {
  const imageUrls = normalizeImageUrls(ad);
  const [selectedImageIndex, setSelectedImageIndex] = React.useState(0);
  const primaryImage = imageUrls[selectedImageIndex] || imageUrls[0];
  const showDetailsButton = !ad.hideDetailsButton;
  const showImagePicker = !!ad.enableImagePicker && imageUrls.length > 1;

  return React.createElement(
    "article",
    { className: "ad-card-react" },
    primaryImage
      ? React.createElement(
          React.Fragment,
          null,
          React.createElement(
            "div",
            { className: "ad-card-react__image-wrap" },
            React.createElement("img", {
              className: "ad-card-react__image",
              src: primaryImage,
              alt: ad.title,
              loading: "lazy",
            })
          ),
          showImagePicker
            ? React.createElement(
                "div",
                { className: "ad-card-react__thumbs", role: "list", "aria-label": "Ad images" },
                imageUrls.map((imageUrl, index) =>
                  React.createElement(
                    "button",
                    {
                      key: `${ad.id}-${index}`,
                      type: "button",
                      className:
                        "ad-card-react__thumb" +
                        (index === selectedImageIndex ? " ad-card-react__thumb--active" : ""),
                      onClick: () => setSelectedImageIndex(index),
                      "aria-label": `Show image ${index + 1}`,
                      "aria-pressed": index === selectedImageIndex,
                    },
                    React.createElement("img", {
                      src: imageUrl,
                      alt: `${ad.title} image ${index + 1}`,
                      loading: "lazy",
                    })
                  )
                )
              )
            : null
        )
      : null,
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
    showDetailsButton
      ? React.createElement(
          "a",
          { className: "ad-card-react__button", href: ad.detailsUrl },
          "View Details"
        )
      : null
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