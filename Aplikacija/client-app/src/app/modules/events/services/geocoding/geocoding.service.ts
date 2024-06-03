import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {SearchResult} from "../../interfaces/search-result";

@Injectable({
  providedIn: 'root'
})
export class GeocodingService {
  private key = 'd96hSHCLErl8rkEEoIrM';
  private apiUrl = 'https://api.maptiler.com/geocoding/';
  private bbox = [21.652477, 43.259756, 22.065391, 43.393398];

  constructor(private http: HttpClient) { }

  searchByName(name: string){
    const url = `${this.apiUrl}${name}.json?bbox=${this.bbox}&key=${this.key}`;
    return this.http.get<SearchResult>(url, {headers:{skip:"true"}});
  }
}
