import { defineConfig } from "vite";
import { resolve } from "path";

export default defineConfig({
  build: {
    outDir: "wwwroot/dist",
    emptyOutDir: true,
    rollupOptions: {
      input: {
        "ad-card-list": resolve(__dirname, "wwwroot/js/ad-card-list.js")
      },
      output: {
        entryFileNames: "[name].js"
      }
    }
  }
});
