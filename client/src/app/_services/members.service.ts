import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { of } from 'rxjs';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { Photo } from '../_models/photo';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  // TODO: Understand this
  memberCache = new Map();
  user = this.accountService.currentUser();
  // TODO: Understand this
  userParams = signal<UserParams>(new UserParams(this.user));

  resetUserParams() {
    this.userParams.set(new UserParams(this.user));
  }
  getMembers() {
    // TODO: Understand this
    const response = this.memberCache.get(
      Object.values(this.userParams()).join('-')
    );
    if (response) return setPaginatedResponse(response, this.paginatedResult);
    let params = setPaginationHeaders(
      this.userParams().pageNumber,
      this.userParams().pageSize
    );

    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy);
    return this.http
      .get<Member[]>(this.baseUrl + '/users', { observe: 'response', params })
      .subscribe({
        next: (response) => {
          setPaginatedResponse(response, this.paginatedResult);
          // TODO: Understand this
          this.memberCache.set(
            Object.values(this.userParams()).join('-'),
            response
          );
        },
      });
  }

  getMember(username: string) {
    // TODO: Understand this
    const member: Member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [])
      .find((m: Member) => m.userName === username);
    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl + '/users/' + username);
  }

  updateMember(member: Member) {
    return this.http
      .put(this.baseUrl + '/users', member)
      .pipe
      // tap(() => {
      //   this.members.update((members) =>
      //     members.map((m) => (m.username === member.username ? member : m))
      //   );
      // })
      ();
  }
  setMainPhoto(photo: Photo) {
    return this.http
      .put(this.baseUrl + '/users/set-main-photo/' + photo.id, {})
      .pipe
      // tap(() => {
      //   this.members.update((members) =>
      //     members.map((m) => {
      //       if (m.photos.includes(photo)) {
      //         m.photoUrl = photo.url;
      //       }
      //       return m;
      //     })
      //   );
      // })
      ();
  }

  deletePhoto(photo: Photo) {
    return this.http
      .delete(this.baseUrl + '/users/delete-photo/' + photo.id)
      .pipe
      // tap(() => {
      //   this.members.update((members) =>
      //     members.map((m) => {
      //       if (m.photos.includes(photo)) {
      //         m.photos = m.photos.filter((x) => x.id !== photo.id);
      //       }
      //       return m;
      //     })
      //   );
      // })
      ();
  }
}
