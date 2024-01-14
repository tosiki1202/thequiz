// ----------------------------------------------------------------------------
// <copyright file="CustomTypes.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
// Sets up support for Unity-specific types. Can be a blueprint how to register your own Custom Types for sending.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------

namespace Photon.Pun
{
    using UnityEngine;
    using Photon.Realtime;
    using ExitGames.Client.Photon;
    using System.Text;

    public struct Question
    {
        public string sentence;
        public string sel_1;
        public string sel_2;
        public string sel_3;
        public string sel_4;
        public int answer_index;
    }

    /// <summary>
    /// Internally used class, containing de/serialization method for PUN specific classes.
    /// </summary>
    internal static class CustomTypes
    {
        /// <summary>Register de/serializer methods for PUN specific types. Makes the type usable in RaiseEvent, RPC and sync updates of PhotonViews.</summary>
        internal static void Register()
        {
            PhotonPeer.RegisterType(typeof(Player), (byte) 'P', SerializePhotonPlayer, DeserializePhotonPlayer);
            PhotonPeer.RegisterType(typeof(Question), (byte) 'Q', SerializeQuestion, DeserializeQuestion);
        }


        #region Custom De/Serializer Methods

        public static readonly byte[] memPlayer = new byte[4];
        public static readonly byte[] bufferQuestion = new byte[1024];

        private static short SerializePhotonPlayer(StreamBuffer outStream, object customobject)
        {
            int ID = ((Player) customobject).ActorNumber;

            lock (memPlayer)
            {
                byte[] bytes = memPlayer;
                int off = 0;
                Protocol.Serialize(ID, bytes, ref off);
                outStream.Write(bytes, 0, 4);
                return 4;
            }
        }
        private static short SerializeQuestion(StreamBuffer outStream, object customobject)
        {
            Question q = (Question)customobject;
            int index = 0;
            lock(bufferQuestion)
            {
                //q.sentenceをバイト列に変換し、bufferQuestionに書き込む refキーワードは参照渡し
                MyProtocol.Serialize(q.sentence, bufferQuestion, ref index);
                MyProtocol.Serialize(q.sel_1, bufferQuestion, ref index);
                MyProtocol.Serialize(q.sel_2, bufferQuestion, ref index);
                MyProtocol.Serialize(q.sel_3, bufferQuestion, ref index);
                MyProtocol.Serialize(q.sel_4, bufferQuestion, ref index);
                Protocol.Serialize(q.answer_index, bufferQuestion, ref index);
                outStream.Write(bufferQuestion, 0, index);
                return (short)index;
            }
        }

        private static object DeserializePhotonPlayer(StreamBuffer inStream, short length)
        {
            if (length != 4)
            {
                return null;
            }

            int ID;
            lock (memPlayer)
            {
                inStream.Read(memPlayer, 0, length);
                int off = 0;
                Protocol.Deserialize(out ID, memPlayer, ref off);
            }

            if (PhotonNetwork.CurrentRoom != null)
            {
                Player player = PhotonNetwork.CurrentRoom.GetPlayer(ID);
                return player;
            }
            return null;
        }
        private static object DeserializeQuestion(StreamBuffer inStream, short length)
        {
            string qu,s1,s2,s3,s4;
            int ans;
            int index = 0;
            Question q = new Question();
            lock (bufferQuestion)
            {
                inStream.Read(bufferQuestion, 0, length);
                //outは、qで呼び出しつつ値を更新する
                MyProtocol.Deserialize(out qu, bufferQuestion, ref index);
                MyProtocol.Deserialize(out s1, bufferQuestion, ref index);
                MyProtocol.Deserialize(out s2, bufferQuestion, ref index);
                MyProtocol.Deserialize(out s3, bufferQuestion, ref index);
                MyProtocol.Deserialize(out s4, bufferQuestion, ref index);
                Protocol.Deserialize(out ans, bufferQuestion, ref index);
            }

            q.sentence = qu;
            q.sel_1 = s1;
            q.sel_2 = s2;
            q.sel_3 = s3;
            q.sel_4 = s4;
            q.answer_index = ans;
            return (Question)q;
        }
        

        #endregion
    }

    public static partial class MyProtocol
    {
        // UTF-8でエンコード・デコードできない文字は空文字に置き換える設定にしておく
        private static readonly Encoding encoding = Encoding.GetEncoding(
            "utf-8",
            new EncoderReplacementFallback(string.Empty),
            new DecoderReplacementFallback(string.Empty)
        );

        //string型の値を指定されたバイト列に書き込む
        //stringは、refキーワードがなくても参照渡しなので呼び出し元のbufferに書き込まれる
        public static void Serialize(string value, byte[] target, ref int offset)
        {
            //GetByte()について、第4には結果が格納される。offset=0なら、target[1]から書き始める.
            int byteCount = encoding.GetBytes(value, 0, value.Length, target, offset + 1);
            byte size = (byte)Mathf.Min(byteCount, byte.MaxValue);
            target[offset] = size;
            offset += size + 1;
        }

        public static void Deserialize(out string value, byte[] source, ref int offset)
        {
            byte size = source[offset];
            value = encoding.GetString(source, offset + 1, size);
            offset += size + 1;
        }
}
}

