USE KDKMusic

INSERT INTO [Admin] ([User_name], [Password], [E_mail], [FullName]) VALUES ('admin123', '123456','trungkiendoan10082002@gmail.com', N'Đoàn Trung Kiên');
SELECT * FROM [Admin]

INSERT INTO [User] ([User_name], [Password], [E_mail]) VALUES ('jika10082k2', '2k21008jika','Jikalizgamer@gmail.com');
SELECT * FROM [User]

INSERT INTO Country ([Country_name]) VALUES ('US')
INSERT INTO Country ([Country_name]) VALUES ('KOREAN')
INSERT INTO Country ([Country_name]) VALUES ('VIETNAM')
SELECT * FROM Country


INSERT INTO Artist(Country_Id, Artist_Name, Artist_Image, Artist_Info) VALUES (2, 'NewJeans', '~/Assets/Mine/images/NewJeans.jpg', N'NewJeans là nhóm nhạc nữ Hàn Quốc được thành lập bởi ADOR, một công ty con của Hybe Corporation. Nhóm bao gồm 5 thành viên: Minji, Hanni, Danielle, Haerin và Hyein.')
GO
INSERT INTO Artist(Country_Id, Artist_Name, Artist_Image, Artist_Info) VALUES (1, 'Alan Walker', '~/Assets/Mine/images/Alan_Walker.jpg', N'Alan Olav Walker, thường được biết đến với nghệ danh Alan Walker là một nam DJ và nhà sản xuất thu âm người Anh gốc Na Uy Vào năm 2015, Alan bắt đầu trở nên nổi tiếng trên phạm vi quốc tế sau khi phát hành đĩa đơn "Faded" và nhận được chứng nhận bạch kim tại 14 quốc gia.')
GO
SELECT * FROM [Artist]

INSERT INTO Music_Genre (Genre_Name) VALUES (N'POP');
GO
INSERT INTO Music_Genre (Genre_Name) VALUES (N'EDM');
GO
SELECT * FROM Music_Genre


INSERT INTO Song(Genre_Id, Artist_Id, Song_Name ,Song_Path, Song_Image, Lyrics) VALUES (1, 1, 'Ditto', '~/Assets/Mine/musics/Ditto.mp3'
, '~/Assets/Mine/imagesForSong/New_Jeans_(EP).jpg'
, N'Hoo-ooh-ooh-ooh
Hoo-ooh-ooh-ooh
Stay in the middle
Like you a little
Don''t want no riddle
말해줘 say it back, oh, say it ditto
아침은 너무 멀어 so say it ditto
훌쩍 커버렸어
함께한 기억처럼
널 보는 내 마음은
어느새 여름 지나 가을
기다렸지 all this time
Do you want somebody
Like I want somebody?
날 보고 웃었지만
Do you think about me now? yeah
All the time, yeah, all the time
I got no time to lose
내 길었던 하루, 난 보고 싶어
Ra-ta-ta-ta 울린 심장 (Ra-ta-ta-ta)
I got nothing to lose
널 좋아한다고 ooh-whoa, ooh-whoa, ooh-whoa
Ra-ta-ta-ta 울린 심장 (Ra-ta-ta-ta)
But I don''t want to
Stay in the middle
Like you a little
Don''t want no riddle
말해줘 say it back, oh, say it ditto
아침은 너무 멀어 so say it ditto
I don''t want to walk in this 미로
다 아는 건 아니어도
바라던 대로 말해줘 say it back
Oh, say it ditto
I want you so, want you, so say it ditto
Not just anybody
너를 상상했지
항상 닿아있던
처음 느낌 그대로 난
기다렸지 all this time
I got nothing to lose
널 좋아한다고 ooh-whoa, ooh-whoa, ooh-whoa
Ra-ta-ta-ta 울린 심장 (Ra-ta-ta-ta)
But I don''t want to
Stay in the middle
Like you a little
Don''t want no riddle
말해줘 say it back, oh, say it ditto
아침은 너무 멀어 so say it ditto
I don''t want to walk in this 미로
다 아는 건 아니어도
바라던 대로 말해줘 say it back
Oh, say it ditto
I want you so, want you, so say it ditto
Hoo-ooh-ooh-ooh
Hoo-ooh-ooh-ooh')
GO
INSERT INTO Song(Genre_Id, Artist_Id, Song_Name ,Song_Path, Song_Image, Lyrics) VALUES (2, 2, 'Faded', '~/Assets/Mine/musics/Fade.mp3'
, '~/Assets/Mine/imagesForSong/Alan_Walker_-_Faded.png'
, N'
You were the shadow to my light
Did you feel us
Another start
You fade away
Afraid our aim is out of sight
Wanna see us
Alive
Where are you now
Where are you now
Where are you now
Was it all in my fantasy
Where are you now
Were you only imaginary

Where are you now
Atlantis
Under the sea
Under the sea
Where are you now
Another dream
The monsters running wild inside of me
I’m faded
I’m faded
So lost, I’m faded
I’m faded
So lost, I’m faded

These shallow waters, never met
What I needed
I’m letting go – a deeper dive
Eternal silence of the sea – I’m breathing
Alive
Where are you now
Where are you now
Under the bright – but faded lights
You’ve set my heart on fire
Where are you now
Where are you now

Where are you now
Atlantis
Under the sea
Under the sea
Where are you now
Another dream
The monster running wild inside of me
I’m faded
I’m faded
So lost, I’m faded
I’m faded
So lost, I’m faded')
GO

SELECT * FROM [Song]

