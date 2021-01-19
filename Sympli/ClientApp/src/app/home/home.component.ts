import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})

export class HomeComponent implements OnInit {
  public baseUrl: string;
  public http: HttpClient;
  public result: SearchResult[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
  }

  ngOnInit() {
    this.Search();
  }

  public Search() {
    this.http.get<any>(this.baseUrl + 'search')
      .subscribe(result => {
        //console.log(result);
        this.result = result;
      }, error => console.error(error));
  }
}

interface SearchResult {
  keyword: string;
  source: string;
  rank: number;
}
