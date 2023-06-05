import { defineConfig } from 'vite';
import path from "node:path";
import process from "node:process";
import solidPlugin from 'vite-plugin-solid';

export default defineConfig({
  plugins: [solidPlugin()],
  server: {
    port: 3000,
  },
  root: 'src/html/',
  publicDir: "../public",
  resolve: {
    alias: { "/src": path.resolve(process.cwd(), "src") }
  },
  build: {
    target: 'esnext',
    outDir: "../../dist",
    emptyOutDir: true,
    rollupOptions: {
      input: {
        main: path.resolve(process.cwd(), "src/html/index.html"),
        baende: path.resolve(process.cwd(), "src/html/baende/index.html"),
      }
    }
  },
});