export interface SearchResult{
  type: string;
  features: {
    id: string;
    text: string;
    type: string;
    geometry: {
      coordinates: number[];
      type: string;
    };
    bbox: number[];
    center: number[];
    place_name: string;
    place_type: string[];
    address: string;
    relevance: number;
  }[];
  query: string[];
  attribution: string;
}
