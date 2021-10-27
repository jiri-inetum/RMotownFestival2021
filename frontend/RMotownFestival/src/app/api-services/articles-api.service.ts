import { Injectable } from '@angular/core';
import { Schedule } from '../api/models/schedule.model';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Artist } from '../api/models/artist.model';
import { Article } from '../api/models/article.model';

@Injectable({
  providedIn: 'root'
})
export class ArticlesApiService {
  private baseUrl = environment.apiBaseUrl + 'articles';

  constructor(private httpClient: HttpClient) { }

  getArticles(): Observable<Article> {
    return this.httpClient.get<Article>(`${this.baseUrl}`);
  }

  postArticle(): Observable<Article> {
    return null;
  }
}
