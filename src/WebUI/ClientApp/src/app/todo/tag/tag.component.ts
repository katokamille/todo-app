import { Component, TemplateRef, Input, Output, EventEmitter } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateTagCommand, TagDto, TagsClient } from 'src/app/web-api-client';


@Component({
  selector: 'app-tag-component',
  templateUrl: './tag.component.html',
  styleUrls: ['./tag.component.scss']
})
export class TagComponent {
  @Input() itemId: number;
  @Input() tags: TagDto[];
  @Output() tagsChange = new EventEmitter<TagDto[]>();
  @Output() onCloseTagModal = new EventEmitter<void>();

  itemTags: TagDto[];
  modalRef: BsModalRef;
  newTagName: string;
  disabled:boolean = false;

  constructor(
    private tagsClient: TagsClient,
    private modalService: BsModalService
  ) { }

  showTagModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  createTag() {
    this.disabled = true;
    const tag = <CreateTagCommand>{
      itemId:  this.itemId,
      name: this.newTagName
    };
    this.tagsClient.create(tag).subscribe(
      result => {
        this.tags.push(<TagDto>{
          id: result,
          itemId: this.itemId,
          name: tag.name
        });
        this.onAfterCreateTag(); 
      }, 
      () => { this.onAfterCreateTag(); },
    )
  }

  onAfterCreateTag() {
    this.newTagName = "";
    this.disabled = false;
  }

  onCloseModal(){
    this.modalRef.hide();
    this.onCloseTagModal.emit();
  }

  onClickRemoveTag(tagId: number) {
    this.tagsClient.delete(tagId).subscribe(
      () => {
        const index = this.tags.findIndex((tag) => tag.id === tagId);

        if (index > -1) 
          this.tags.splice(index, 1);
      }
    );
  }

}